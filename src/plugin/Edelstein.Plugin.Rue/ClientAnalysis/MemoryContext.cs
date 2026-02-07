using Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

/// <summary>
/// Shared context holding the MemoryWriter and LoginStepMonitor.
/// Created once by RueLoginPlugin and passed to all auto-login plugs.
/// Initialization is lazy — the first plug that calls TryInitialize() attaches
/// to the client process and starts the monitor.
/// </summary>
public sealed class MemoryContext : IDisposable
{
    private MemoryWriter? _writer;
    private LoginStepMonitor? _monitor;
    private bool _initialized;
    private bool _initializing;
    private bool _disposed;
    private readonly object _lock = new();

    public MemoryWriter? Writer => _writer;
    public LoginStepMonitor? Monitor => _monitor;
    public LoginMilestonesTracker? Tracker { get; set; }

    /// <summary>
    /// Lazily initializes the memory writer and monitor.
    /// Thread-safe and idempotent — only the first call does work.
    /// Returns true if the writer is available for use.
    /// </summary>
    public bool TryInitialize(RueConfigClientMemory config, ILogger? logger, LoginDiagnostics? diagnostics, bool diagnosticsEnabled)
    {
        lock (_lock)
        {
            if (_disposed)
                return false;
            if (_initialized)
                return _writer != null;
            if (_initializing)
                return false;
            _initializing = true;
        }

        var memLogger = logger != null ? new DiagnosticLogger(logger, diagnosticsEnabled) : null;
        var writer = new MemoryWriter(memLogger, config.ProcessName);
        if (!writer.TryAttach())
        {
            logger?.LogError("[Rue-AutoLogin] Failed to attach to {ProcessName}", config.ProcessName);
            lock (_lock)
            {
                _initializing = false;
            }
            return false;
        }

        var mode = config.UseDirectFunctionCall ? "DFC" : "manual-write";
        Tracker?.RecordWriterAttached(mode);

        // Create and start the monitor
        var monitor = new LoginStepMonitor(
            writer, logger, diagnostics, Tracker,
            config.MonitorPollIntervalMs,
            config.MonitorTimeoutMs);
        monitor.Start();

        Tracker?.RecordMonitorStarted(config.MonitorPollIntervalMs, config.MonitorTimeoutMs);

        lock (_lock)
        {
            _writer = writer;
            _monitor = monitor;
            _initialized = true;
            _initializing = false;
        }

        return true;
    }

    /// <summary>
    /// Dumps and analyzes the SendLoginPacket function chain at runtime.
    /// Creates a RuntimeFunctionDumper, registers known addresses, runs a BFS
    /// chain analysis following external call targets, and saves the report.
    /// </summary>
    /// <returns>File path of the saved report, or null on failure.</returns>
    public string? DumpSendLoginPacketChain(RueConfigClientMemory config, ILogger? logger)
    {
        if (_writer == null)
        {
            logger?.LogError("[Rue-AutoLogin] Cannot dump functions — writer not initialized");
            return null;
        }

        try
        {
            var dumper = new RuntimeFunctionDumper(_writer.ProcessHandle, logger);

            foreach (var kv in V95ClientStructs.Singletons)
                dumper.RegisterKnownPointer(kv.Key, kv.Value);

            var funcAddr = V95ClientStructs.Addresses.SendLoginPacketFunc;
            dumper.RegisterKnownPointer(funcAddr, "CLogin::SendLoginPacket");

            var report = dumper.AnalyzeFunctionChain(
                "CLogin::SendLoginPacket",
                funcAddr,
                new FunctionChainOptions
                {
                    MaxBytesPerFunction = config.DumpMaxBytes,
                    MaxDepth = config.DumpFollowDepth,
                    MaxFunctionsPerLevel = config.DumpMaxFunctionsPerLevel,
                });

            var filePath = LoginDiagnostics.SaveReportToFile(report, "function_dump");
            return filePath;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "[Rue-AutoLogin] Function dump failed");
            return null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        _monitor?.Dispose();
        _writer?.Detach();
    }
}
