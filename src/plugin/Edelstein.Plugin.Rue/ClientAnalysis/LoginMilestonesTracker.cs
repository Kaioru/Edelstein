using Edelstein.Protocol.Gameplay.Login;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class LoginMilestonesTracker
{
    private readonly ILogger? _logger;
    private readonly object _lock = new();
    private LoginState? _lastServerState;
    private int? _lastClientStep;
    private bool? _lastStepStable;
    private long _loginStartTick;

    public LoginMilestonesTracker(ILogger? logger)
    {
        _logger = logger;
    }

    public void StartLogin(string username)
    {
        var now = Environment.TickCount64;

        lock (_lock)
        {
            _loginStartTick = now;
            _lastServerState = null;
            _lastClientStep = null;
            _lastStepStable = null;
        }

        _logger?.LogInformation("[Rue-AutoLogin] Login started for {Username}", string.IsNullOrWhiteSpace(username) ? "unknown" : username);
    }

    public void RecordServerState(LoginState next)
    {
        var now = Environment.TickCount64;
        LoginState? prev;
        long startTick;

        lock (_lock)
        {
            prev = _lastServerState;
            if (prev == next)
                return;

            _lastServerState = next;
            if (_loginStartTick == 0)
                _loginStartTick = now;
            startTick = _loginStartTick;
        }

        _logger?.LogInformation("[Rue-AutoLogin] Server state: {Prev} -> {Next} (+{Elapsed})",
            prev?.ToString() ?? "none",
            next,
            FormatElapsed(now - startTick));
    }

    public void RecordClientStep(int? prev, int? next)
    {
        var now = Environment.TickCount64;
        long startTick;

        lock (_lock)
        {
            if (_lastClientStep == next)
                return;
            _lastClientStep = next;
            if (_loginStartTick == 0)
                _loginStartTick = now;
            startTick = _loginStartTick;
        }

        _logger?.LogInformation("[Rue-AutoLogin] Client step: {Prev} -> {Next} (+{Elapsed})",
            DescribeLoginStep(prev),
            DescribeLoginStep(next),
            FormatElapsed(now - startTick));
    }

    public void RecordClientStepStability(bool stable)
    {
        var now = Environment.TickCount64;
        long startTick;
        bool? prev;

        lock (_lock)
        {
            prev = _lastStepStable;
            if (prev.HasValue && prev.Value == stable)
                return;
            _lastStepStable = stable;
            if (_loginStartTick == 0)
                _loginStartTick = now;
            startTick = _loginStartTick;
        }

        _logger?.LogInformation("[Rue-AutoLogin] Client step: {State} (+{Elapsed})",
            stable ? "stable" : "transitioning",
            FormatElapsed(now - startTick));
    }

    public void RecordSingletonReady(string className, long elapsedMs)
    {
        _logger?.LogInformation("[Rue-AutoLogin] Class {ClassName} came into existence in ~{Elapsed}",
            className,
            FormatElapsed(elapsedMs));
    }

    public void RecordCompletion()
    {
        var now = Environment.TickCount64;
        long startTick;

        lock (_lock)
        {
            if (_loginStartTick == 0)
                _loginStartTick = now;
            startTick = _loginStartTick;
        }

        _logger?.LogInformation("[Rue-AutoLogin] Login complete: connect -> in-game in {Ms}ms", now - startTick);
    }

    public void RecordWriterAttached(string mode)
    {
        _logger?.LogInformation("[Rue-AutoLogin] Memory writer attached (mode={Mode})", mode);
    }

    public void RecordMonitorStarted(int pollIntervalMs, int timeoutMs)
    {
        _logger?.LogInformation("[Rue-AutoLogin] Monitor started (poll={PollMs}ms, timeout={TimeoutMs}ms)", pollIntervalMs, timeoutMs);
    }

    public static string FormatElapsed(long ms)
    {
        return ms <= 0 ? "<1ms" : $"{ms}ms";
    }

    private static string DescribeLoginStep(int? step)
    {
        if (!step.HasValue)
            return "unknown";

        var raw = step.Value;
        if (raw < byte.MinValue || raw > byte.MaxValue)
            return raw.ToString();

        var value = (byte)raw;
        if (Enum.IsDefined(typeof(LoginStep), value))
            return $"{raw} ({(LoginStep)value})";

        return raw.ToString();
    }
}
