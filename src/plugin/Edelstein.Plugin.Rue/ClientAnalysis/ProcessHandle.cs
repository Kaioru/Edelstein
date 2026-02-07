using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class ProcessHandle(ILogger? logger) : IDisposable
{
    private readonly ILogger? _logger = logger;
    private IntPtr _handle = IntPtr.Zero;
    private int _processId;
    private bool _processExited;

    private const Win32Api.ProcessAccess Access =
        Win32Api.ProcessAccess.VmRead |
        Win32Api.ProcessAccess.VmWrite |
        Win32Api.ProcessAccess.VmOperation |
        Win32Api.ProcessAccess.QueryInformation;

    public IntPtr Handle => _handle;
    public int ProcessId => _processId;
    public bool HasExited => _processExited;
    public bool IsAttached => _handle != IntPtr.Zero;

    public bool TryAttach(string processName)
    {
        if (string.IsNullOrWhiteSpace(processName))
        {
            _logger?.LogWarning("[Rue-CMemory] ProcessName not configured");
            return false;
        }

        // 1. Normalize input: "localhost.exe" -> "localhost"
        // Process.GetProcessesByName requires the name without the extension.
        var targetName = processName.Trim();
        if (targetName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            targetName = targetName[..^4];
        }

        // 2. Find the process directly
        var processes = Process.GetProcessesByName(targetName);

        if (processes.Length == 0)
        {
            _logger?.LogWarning("[Rue-CMemory] No process found matching '{ProcessName}'", targetName);
            return false;
        }

        // 3. Select the first match
        var match = processes[0];

        // Dispose of any extra matches immediately if multiple clients are open
        for (var i = 1; i < processes.Length; i++)
        {
            processes[i].Dispose();
        }

        try
        {
            _processId = match.Id;
            
            // Grab these before opening the handle/disposing the wrapper, just for logging
            var actualName = match.ProcessName; 
            var title = match.MainWindowTitle;

            _handle = Win32Api.OpenProcess(Access, false, _processId);

            if (_handle == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                _logger?.LogError("[Rue-CMemory] Failed to open process. Error: {Error}", error);
                return false;
            }

            _logger?.LogDebug(
                "[Rue-CMemory] Attached to {ProcessName} (PID: {PID}) Title='{Title}'",
                actualName, 
                _processId, 
                title
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[Rue-CMemory] Exception while attaching to process");
            return false;
        }
        finally
        {
            // Always dispose the Process component wrapper; we have the native handle now
            match.Dispose();
        }
    }

    public void Detach()
    {
        if (_handle == IntPtr.Zero)
            return;

        Win32Api.CloseHandle(_handle);
        _handle = IntPtr.Zero;
        _processId = 0;
        _processExited = false;
    }

    public bool TryMarkExitedFromError(int error)
        => error == 299 && DetectProcessExit();

    private bool DetectProcessExit()
    {
        try
        {
            var proc = Process.GetProcessById(_processId);
            if (proc.HasExited)
            {
                _processExited = true;
                _logger?.LogWarning("[Rue-CMemory] Client process (PID {PID}) has exited", _processId);
                return true;
            }
        }
        catch
        {
            _processExited = true;
            _logger?.LogWarning("[Rue-CMemory] Client process (PID {PID}) no longer exists", _processId);
            return true;
        }

        return false;
    }

    public void Dispose()
    {
        Detach();
    }
}
