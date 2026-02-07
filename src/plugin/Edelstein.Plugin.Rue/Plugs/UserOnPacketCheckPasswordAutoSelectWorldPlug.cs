using System.Collections.Concurrent;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Diagnostics;
using Edelstein.Plugin.Rue.Login.WorldSelect;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

/// <summary>
/// Automatically selects world/channel when client requests world info (auto-login feature).
/// Runs AFTER the normal WorldRequestPlug sends the world list.
/// </summary>
public class UserOnPacketCheckPasswordAutoSelectWorldPlug : IPipelinePlug<UserOnPacketWorldRequest>
{
    private readonly ILogger? _logger;
    private readonly RueConfigLogin _config;
    private readonly LoginContext _context;
    private readonly LoginDiagnostics? _diagnostics;
    private readonly MemoryContext? _memoryContext;
    private readonly LoginMilestonesTracker? _tracker;
    private readonly IWorldSelectStrategy _directStrategy = new DirectFunctionCallStrategy();
    private readonly IWorldSelectStrategy _manualStrategy = new ManualMemoryWriteStrategy();

    // Prevent duplicate execution (WorldRequest and WorldInfoRequest both use this pipeline)
    private readonly ConcurrentDictionary<int, byte> _processingUsers = new();

    public UserOnPacketCheckPasswordAutoSelectWorldPlug(
        ILogger? logger,
        IOptions<RueConfigLogin> options,
        LoginContext context,
        LoginDiagnostics? diagnostics = null,
        MemoryContext? memoryContext = null,
        LoginMilestonesTracker? tracker = null)
    {
        _logger = logger;
        _config = options.Value;
        _context = context;
        _diagnostics = diagnostics;
        _memoryContext = memoryContext;
        _tracker = tracker;
    }

    private MemoryWriter? Writer => _memoryContext?.Writer;
    private LoginStepMonitor? Monitor => _memoryContext?.Monitor;

    public async Task Handle(IPipelineContext ctx, UserOnPacketWorldRequest message)
    {
        if (ctx.IsRequestedCancellation)
            return;

        if (!_config.IsAutoLogin)
            return;

        if (_config.AutoSelectWorldID == null || _config.AutoSelectChannelID == null)
            return;

        // In passive mode, skip auto-login intervention but let diagnostics run
        var watchMode = _config.ClientMemory?.WatchMode ?? RueConfigClientMemory.WatchModeActive;
        if (watchMode.Equals(RueConfigClientMemory.WatchModePassive, StringComparison.OrdinalIgnoreCase))
        {
            _logger?.LogInformation("[Rue-AutoLogin] Passive mode - skipping world select for {Username}", message.User.Account?.Username);
            _diagnostics?.RecordMilestone("PassiveModeStart");
            return;
        }

        if (message.User.State != LoginState.SelectWorld)
        {
            _logger?.LogWarning("[Rue-AutoLogin] Skipping - state is {State}, expected SelectWorld", message.User.State);
            return;
        }

        _tracker?.RecordServerState(message.User.State);

        // Prevent duplicate execution (WorldRequest and WorldInfoRequest both trigger this)
        var accountId = message.User.Account?.ID ?? 0;
        if (!_processingUsers.TryAdd(accountId, 0))
            return;

        try
        {
            var t0 = Environment.TickCount64;
            var tLast = t0;
            var delay = _config.AutoSelectDelayMs;

            _diagnostics?.RecordMilestone("WorldInformation");

            // Ensure memory context is initialized (idempotent — may have been initialized by CreateSecurityHandlePlug)
            var clientMemoryEnabled = _config.ClientMemory?.Enabled ?? false;
            if (clientMemoryEnabled && _memoryContext != null)
                _memoryContext.TryInitialize(_config.ClientMemory!, _logger, _diagnostics, _config.DiagnosticsEnabled);

            _logger?.LogInformation("[Rue-AutoLogin] Selecting world {WorldID}, channel {ChannelID}",
                _config.AutoSelectWorldID, _config.AutoSelectChannelID);

            // Wait for CUIWorldSelect to exist before sending CheckUserLimitResult
            // If CUIWorldSelect is NULL when CheckUserLimitResult arrives, client crashes
            if (Monitor != null)
            {
                var worldSelectOk = await Monitor.WaitWithTimeout(
                    ct => Monitor.WaitForSingleton("CUIWorldSelect", ct),
                    "CUIWorldSelect");

                if (!worldSelectOk)
                    _logger?.LogWarning("[Rue-AutoLogin] CUIWorldSelect timeout - proceeding anyway");
                else
                    _diagnostics?.RecordMilestone("CUIWorldSelectCreated");
            }
            else if (delay > 0)
            {
                _logger?.LogInformation("[Rue-AutoLogin] Waiting {Delay}ms for WorldInformation (delay-based)", delay);
                await Task.Delay(delay);
                _diagnostics?.RecordMilestone("CUIWorldSelectCreated");
            }

            var tNow = Environment.TickCount64;
            _tracker?.RecordSingletonReady("CUIWorldSelect", tNow - tLast);
            tLast = tNow;

            var clientMemoryConfig = _config.ClientMemory;
            if ((clientMemoryConfig?.FixupWorldSelectWorldIdx ?? true) && Writer != null)
            {
                var worldId = _config!.AutoSelectWorldID!.Value;

                _diagnostics?.RecordMilestone("SetWorldSelectWorldIdx");
                _diagnostics?.LogMemoryWrite("CUIWorldSelect->m_nWorldIdx", -1, worldId);

                if (Writer.SetWorldSelectWorldIdx(worldId))
                    _logger?.LogInformation("[Rue-AutoLogin] Set CUIWorldSelect.m_nWorldIdx={WorldId}", worldId);
                else
                    _logger?.LogWarning("[Rue-AutoLogin] Failed to set CUIWorldSelect.m_nWorldIdx");
            }

            _logger?.LogInformation("[Rue-AutoLogin] Sending CheckUserLimitResult");
            _diagnostics?.RecordMilestone("CheckUserLimitResult");
            _diagnostics?.LogPacketSent("CheckUserLimitResult", (int)PacketSendOperations.CheckUserLimitResult,
                new Dictionary<string, object?> { ["bOverUserLimit"] = 0, ["bPopulateLevel"] = 0 });

            using (var checkUserLimitPacket = new PacketWriter(PacketSendOperations.CheckUserLimitResult))
            {
                checkUserLimitPacket.WriteByte(0); // bOverUserLimit = 0
                checkUserLimitPacket.WriteByte(0); // bPopulateLevel = 0
                await message.User.Dispatch(checkUserLimitPacket.Build());
            }

            // Wait for CUIChannelSelect creation
            if (Monitor != null)
            {
                var channelSelectOk = await Monitor.WaitWithTimeout(
                    ct => Monitor.WaitForSingleton("CUIChannelSelect", ct),
                    "CUIChannelSelect");

                if (!channelSelectOk)
                    _logger?.LogWarning("[Rue-AutoLogin] CUIChannelSelect timeout - proceeding anyway");
                else
                    _diagnostics?.RecordMilestone("CUIChannelSelectCreated");
            }
            else if (delay > 0)
            {
                await Task.Delay(delay);
                _diagnostics?.RecordMilestone("CUIChannelSelectCreated");
            }

            tNow = Environment.TickCount64;
            _tracker?.RecordSingletonReady("CUIChannelSelect", tNow - tLast);
            tLast = tNow;

            if (clientMemoryEnabled)
            {
                await HandleWithClientMemory(message.User);
            }
            else
            {
                _logger?.LogInformation("[Rue-AutoLogin] Channel select ready - click channel to continue (no ClientMemory)");
            }

        }
        finally
        {
            _processingUsers.TryRemove(accountId, out _);
        }
    }

    /// <summary>
    /// Full auto-login using client memory modification.
    /// Two approaches:
    /// 1. UseDirectFunctionCall=true: Calls CLogin::SendLoginPacket remotely via shellcode injection.
    /// 2. UseDirectFunctionCall=false: Manually writes CWvsContext fields and triggers pipeline.
    /// </summary>
    private async Task HandleWithClientMemory(ILoginStageUser user)
    {
        if (Writer == null)
        {
            _logger?.LogError("[Rue-AutoLogin] ClientMemory writer not available - check config and permissions");
            return;
        }

        if (!Writer.FindCWvsContext())
        {
            _logger?.LogError("[Rue-AutoLogin] Failed to find CWvsContext");
            return;
        }

        // Safety check: CUIChannelSelect should exist (monitor already waited)
        if (Writer.IsCUIChannelSelectValid() == false)
            _logger?.LogWarning("[Rue-AutoLogin] CUIChannelSelect not found despite waiting");

        var worldId = _config!.AutoSelectWorldID!.Value;
        var channelId = _config.AutoSelectChannelID!.Value;

        var clientMemoryConfig = _config.ClientMemory;
        if (clientMemoryConfig?.DumpFunctionsOnLogin ?? false)
        {
            _logger?.LogInformation("[Rue-AutoLogin] Dumping SendLoginPacket chain for analysis...");
            var reportPath = _memoryContext?.DumpSendLoginPacketChain(clientMemoryConfig, _logger);
            if (reportPath != null)
                _logger?.LogInformation("[Rue-AutoLogin] Function dump saved: {Path}", reportPath);
            else
                _logger?.LogWarning("[Rue-AutoLogin] Function dump failed");
        }

        var useDirectCall = clientMemoryConfig?.UseDirectFunctionCall ?? false;

        var strategyContext = new WorldSelectContext(
            _logger,
            _config!,
            _diagnostics,
            _memoryContext,
            _tracker,
            Writer,
            Monitor,
            _context,
            user,
            worldId,
            channelId);

        var strategy = useDirectCall ? _directStrategy : _manualStrategy;
        await strategy.Execute(strategyContext);
    }

}
