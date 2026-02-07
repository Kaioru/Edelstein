using Edelstein.Plugin.Rue.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

/// <summary>
/// Polls client memory at a fixed interval and provides awaitable signals for
/// client state transitions during the auto-login flow.
///
/// The monitor reads CLogin->m_nLoginStep, CLogin->m_tStepChanging, and
/// UI singleton pointers each tick, then completes registered waiters
/// when their conditions are met.
///
/// Thread safety: All public methods are thread-safe. The polling timer
/// fires on a ThreadPool thread; waiters are completed asynchronously.
/// </summary>
public sealed class LoginStepMonitor(
    MemoryWriter memoryWriter,
    ILogger? logger = null,
    LoginDiagnostics? diagnostics = null,
    LoginMilestonesTracker? tracker = null,
    int pollIntervalMs = 50,
    int defaultTimeoutMs = 10000) : IDisposable
{
    private readonly ILogger? _logger = logger;
    private readonly MemoryWriter _memoryWriter = memoryWriter ?? throw new ArgumentNullException(nameof(memoryWriter));
    private readonly LoginDiagnostics? _diagnostics = diagnostics;
    private readonly LoginMilestonesTracker? _tracker = tracker;
    private readonly int _pollIntervalMs = pollIntervalMs > 0 ? pollIntervalMs : 50;
    private readonly int _defaultTimeoutMs = defaultTimeoutMs > 0 ? defaultTimeoutMs : 10000;

    // Polling state
    private Timer? _timer;
    private readonly object _lock = new();
    private bool _disposed;
    private bool _started;

    // Current known state (updated each tick)
    private bool _loginGradeWndExists;
    private bool _worldSelectExists;
    private bool _channelSelectExists;
    private int? _loginStep;
    private int? _stepChanging;
    private bool _cloginResolved;

    // Waiter lists (completed from timer callback, registered from any thread)
    private readonly List<SingletonWaiter> _singletonWaiters = [];
    private readonly List<LoginStepWaiter> _loginStepWaiters = [];
    private readonly List<StepTransitionWaiter> _stepTransitionWaiters = [];

    private record SingletonWaiter(string Name, TaskCompletionSource<bool> Tcs);
    private record LoginStepWaiter(LoginStep TargetStep, TaskCompletionSource<bool> Tcs);
    private record StepTransitionWaiter(TaskCompletionSource<bool> Tcs);

    /// <summary>
    /// Current client login step, or null if CLogin has not been resolved yet.
    /// </summary>
    public LoginStep? CurrentLoginStep
    {
        get
        {
            lock (_lock)
                return _loginStep.HasValue ? (LoginStep)_loginStep.Value : null;
        }
    }

    /// <summary>
    /// Whether CLogin->m_tStepChanging is 0 (no transition in progress).
    /// Null if CLogin has not been resolved yet.
    /// </summary>
    public bool? IsStepTransitionComplete
    {
        get
        {
            lock (_lock)
                return _stepChanging.HasValue ? _stepChanging.Value == 0 : null;
        }
    }

    public bool? IsStepStable => IsStepTransitionComplete;

    /// <summary>
    /// Starts the polling timer. Idempotent.
    /// </summary>
    public void Start()
    {
        lock (_lock)
        {
            if (_started || _disposed)
                return;

            _started = true;
            _timer = new Timer(OnTick, null, 0, _pollIntervalMs);
        }
    }

    /// <summary>
    /// Stops polling and cancels all pending waiters.
    /// </summary>
    public void Stop()
    {
        List<SingletonWaiter> sWaiters;
        List<LoginStepWaiter> lWaiters;
        List<StepTransitionWaiter> stWaiters;

        lock (_lock)
        {
            if (!_started)
                return;

            _started = false;
            _timer?.Dispose();
            _timer = null;

            // Snapshot waiters to cancel outside the lock
            sWaiters = [.. _singletonWaiters];
            lWaiters = [.. _loginStepWaiters];
            stWaiters = [.. _stepTransitionWaiters];
            _singletonWaiters.Clear();
            _loginStepWaiters.Clear();
            _stepTransitionWaiters.Clear();
        }

        // Cancel all pending waiters
        foreach (var w in sWaiters)
            w.Tcs.TrySetCanceled();
        foreach (var w in lWaiters)
            w.Tcs.TrySetCanceled();
        foreach (var w in stWaiters)
            w.Tcs.TrySetCanceled();

        _logger?.LogDebug("[Rue-Monitor] Monitor stopped");
    }

    /// <summary>
    /// Returns a Task that completes when the named singleton pointer becomes non-NULL.
    /// Supported names: "CLoginGradeWnd", "CUIWorldSelect", "CUIChannelSelect".
    /// If the singleton already exists, returns immediately.
    /// </summary>
    public Task WaitForSingleton(string name, CancellationToken ct = default)
    {
        lock (_lock)
        {
            // Check if already satisfied
            if (name == "CLoginGradeWnd" && _loginGradeWndExists)
                return Task.CompletedTask;
            if (name == "CUIWorldSelect" && _worldSelectExists)
                return Task.CompletedTask;
            if (name == "CUIChannelSelect" && _channelSelectExists)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            ct.Register(() => tcs.TrySetCanceled(ct));
            _singletonWaiters.Add(new SingletonWaiter(name, tcs));
            return tcs.Task;
        }
    }

    /// <summary>
    /// Returns a Task that completes when CLogin->m_nLoginStep reaches the target value.
    /// For example, WaitForLoginStep(LoginStep.SelectCharacter) waits for
    /// the client to enter the character selection screen.
    /// </summary>
    public Task WaitForLoginStep(LoginStep step, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (_loginStep.HasValue && _loginStep.Value >= (int)step)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            ct.Register(() => tcs.TrySetCanceled(ct));
            _loginStepWaiters.Add(new LoginStepWaiter(step, tcs));
            return tcs.Task;
        }
    }

    /// <summary>
    /// Returns a Task that completes when CLogin->m_tStepChanging becomes 0,
    /// indicating the client's step transition animation is complete.
    /// Requires CLogin to be resolved (will wait for it).
    /// </summary>
    public Task WaitForStepTransitionComplete(CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (_loginStep.HasValue && _stepChanging.HasValue && _stepChanging.Value == 0)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            ct.Register(() => tcs.TrySetCanceled(ct));
            _stepTransitionWaiters.Add(new StepTransitionWaiter(tcs));
            return tcs.Task;
        }
    }

    public Task WaitForStepStable(CancellationToken ct = default) => WaitForStepTransitionComplete(ct);

    /// <summary>
    /// Wraps a wait call with the configured default timeout.
    /// Returns true if the condition was met, false if timed out.
    /// </summary>
    public async Task<bool> WaitWithTimeout(Func<CancellationToken, Task> waitFactory, string description)
    {
        using var cts = new CancellationTokenSource(_defaultTimeoutMs);
        try
        {
            await waitFactory(cts.Token);
            _logger?.LogDebug("[Rue-Monitor] {Description} satisfied", description);
            return true;
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            _logger?.LogWarning("[Rue-Monitor] {Description} timeout ({TimeoutMs}ms)", description, _defaultTimeoutMs);
            return false;
        }
    }

    private void OnTick(object? state)
    {
        if (_disposed || !_started)
            return;

        try
        {
            // Read current state from client memory
            var lgwExists = _memoryWriter.IsCLoginGradeWndValid() == true;
            var wsExists = _memoryWriter.IsCUIWorldSelectValid() == true;
            var csExists = _memoryWriter.IsCUIChannelSelectValid() == true;

            // Try to resolve CLogin once CUIChannelSelect exists
            int? step = null;
            int? stepChg = null;

            if (!_cloginResolved && csExists)
            {
                if (_memoryWriter.FindCLogin())
                {
                    _cloginResolved = true;
                    _logger?.LogDebug("[Rue-Monitor] CLogin resolved via CUIChannelSelect");
                }
            }

            if (_cloginResolved)
            {
                step = _memoryWriter.ReadLoginStep();
                stepChg = _memoryWriter.ReadStepChanging();

                // If reads return null, CLogin may have been destroyed
                if (step == null)
                {
                    _cloginResolved = false;
                }
            }

            lock (_lock)
            {
                // Detect changes for logging
                var lgwChanged = lgwExists != _loginGradeWndExists;
                var wsChanged = wsExists != _worldSelectExists;
                var csChanged = csExists != _channelSelectExists;
                var stepChanged = step != _loginStep;

                var prevLoginStep = _loginStep;
                var prevStepChanging = _stepChanging;
                var prevStable = prevStepChanging.HasValue ? prevStepChanging.Value == 0 : (bool?)null;
                var currentStable = stepChg.HasValue ? stepChg.Value == 0 : (bool?)null;

                _loginGradeWndExists = lgwExists;
                _worldSelectExists = wsExists;
                _channelSelectExists = csExists;
                _loginStep = step;
                _stepChanging = stepChg;

                if (lgwChanged)
                    _logger?.LogDebug("[Rue-Monitor] CLoginGradeWnd: {State}", lgwExists ? "created" : "destroyed");
                if (wsChanged)
                    _logger?.LogDebug("[Rue-Monitor] CUIWorldSelect: {State}", wsExists ? "created" : "destroyed");
                if (csChanged)
                    _logger?.LogDebug("[Rue-Monitor] CUIChannelSelect: {State}", csExists ? "created" : "destroyed");
                if (stepChanged)
                    _tracker?.RecordClientStep(prevLoginStep, step);
                if (prevStable != currentStable && currentStable.HasValue)
                    _tracker?.RecordClientStepStability(currentStable.Value);

                // Complete singleton waiters
                for (var i = _singletonWaiters.Count - 1; i >= 0; i--)
                {
                    var w = _singletonWaiters[i];
                    var satisfied = (w.Name == "CLoginGradeWnd" && lgwExists)
                                 || (w.Name == "CUIWorldSelect" && wsExists)
                                 || (w.Name == "CUIChannelSelect" && csExists);
                    if (satisfied)
                    {
                        w.Tcs.TrySetResult(true);
                        _singletonWaiters.RemoveAt(i);
                    }
                }

                // Complete login step waiters
                for (var i = _loginStepWaiters.Count - 1; i >= 0; i--)
                {
                    var w = _loginStepWaiters[i];
                    if (step.HasValue && step.Value >= (int)w.TargetStep)
                    {
                        w.Tcs.TrySetResult(true);
                        _loginStepWaiters.RemoveAt(i);
                    }
                }

                // Complete step-stable waiters
                for (var i = _stepTransitionWaiters.Count - 1; i >= 0; i--)
                {
                    var w = _stepTransitionWaiters[i];
                    if (step.HasValue && stepChg.HasValue && stepChg.Value == 0)
                    {
                        w.Tcs.TrySetResult(true);
                        _stepTransitionWaiters.RemoveAt(i);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "[Rue-Monitor] Error during poll tick");
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        Stop();
    }

}
