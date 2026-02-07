using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Contracts;
using Edelstein.Plugin.Rue.Diagnostics;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCreateSecurityHandleAutoRegisterPlug(
    ILogger? logger,
    IOptions<RueConfigLogin> options,
    LoginContext context,
    MemoryContext? memoryContext = null,
    LoginDiagnostics? diagnostics = null,
    LoginMilestonesTracker? tracker = null) : IPipelinePlug<UserOnPacketCreateSecurityHandle>
{
    private readonly ILogger? _logger = logger;
    private readonly RueConfigLogin _config = options.Value;
    private readonly LoginContext _context = context;
    private readonly MemoryContext? _memoryContext = memoryContext;
    private readonly LoginDiagnostics? _diagnostics = diagnostics;
    private readonly LoginMilestonesTracker? _tracker = tracker;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCreateSecurityHandle message)
    {
        if (!_config.IsAutoLogin || _config.LoginCredentials is not { Username: not null, Password: not null })
            return;

        var t0 = Environment.TickCount64;
        _tracker?.StartLogin(_config.LoginCredentials.Username);
        _tracker?.RecordServerState(message.User.State);

        // Wait for client title screen to be ready before sending CheckPassword
        var clientMemoryEnabled = _config.ClientMemory?.Enabled ?? false;
        var usedMonitor = false;

        if (clientMemoryEnabled && _memoryContext != null)
        {
            if (_memoryContext.TryInitialize(_config.ClientMemory!, _logger, _diagnostics, _config.DiagnosticsEnabled))
            {
                var monitor = _memoryContext.Monitor!;
                var ok = await monitor.WaitWithTimeout(
                    ct => monitor.WaitForSingleton("CLoginGradeWnd", ct),
                    "CLoginGradeWnd");

                if (!ok)
                    _logger?.LogWarning("[Rue-AutoLogin] CLoginGradeWnd timeout - proceeding");

                var elapsed = Environment.TickCount64 - t0;
                _tracker?.RecordSingletonReady("CLoginGradeWnd", elapsed);
                usedMonitor = true;
            }
        }

        if (!usedMonitor)
        {
            // Fallback: delay-based wait when ClientMemory is disabled
            var delay = _config.AutoSelectDelayMs;
            if (delay > 0)
            {
                _logger?.LogInformation("[Rue-AutoLogin] Waiting {Delay}ms for client init (delay-based)", delay);
                await Task.Delay(delay);
            }
        }

        var result = await _context.Pipelines.UserOnPacketCheckPassword.Process(new UserOnPacketCheckPasswordFlipped(
            message.User,
            _config.LoginCredentials.Username,
            _config.LoginCredentials.Password
        ));

        _diagnostics?.RecordMilestone("CheckPasswordResult");
    }
}
