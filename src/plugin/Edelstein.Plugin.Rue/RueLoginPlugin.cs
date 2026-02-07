using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Diagnostics;
using Edelstein.Plugin.Rue.Plugs;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Login;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue;

/// <summary>
/// Login server plugin providing auto-registration, auto-login, and auto-character features.
/// </summary>
public class RueLoginPlugin : ILoginPlugin
{
    public string ID => "RueLogin";

    private ILogger? Logger { get; set; }
    private IOptions<RueConfigLogin> _options = Options.Create(new RueConfigLogin());

    // Shared diagnostics instance for all plugs
    private LoginDiagnostics? _diagnostics;
    private UserOnPacketDiagnosticsPlug? _diagnosticsPlug;

    // Shared context holding writer + monitor, lazily initialized by first plug that needs it
    private readonly MemoryContext _memoryContext = new();

    public Task OnInit(IPluginHost<LoginContext> host, LoginContext ctx)
    {
        Logger = host.Logger;
        return Task.CompletedTask;
    }

    public Task OnStart(IPluginHost<LoginContext> host, LoginContext ctx)
    {
        _options = RueConfigBinding.BindLoginOptions(host.Config);
        var options = _options;

        _diagnostics = new LoginDiagnostics();
        var tracker = new LoginMilestonesTracker(Logger);
        _memoryContext.Tracker = tracker;

        if (options.Value.DiagnosticsEnabled)
        {
            _diagnosticsPlug = new UserOnPacketDiagnosticsPlug(Logger, options, _diagnostics, _memoryContext);
            ctx.Pipelines.UserOnPacket.Add(PipelinePriority.Highest, _diagnosticsPlug);
        }

        ctx.Pipelines.UserOnPacketCreateSecurityHandle.Add(PipelinePriority.High, new UserOnPacketCreateSecurityHandleAutoRegisterPlug(
            Logger,
            options,
            ctx,
            _memoryContext,
            _diagnostics,
            tracker
        ));

        ctx.Pipelines.UserOnPacketCheckPassword.Add(PipelinePriority.High, new UserOnPacketCheckPasswordAutoLoginPlug(
            Logger,
            options,
            ctx
        ));

        ctx.Pipelines.UserOnPacketCheckPassword.Add(PipelinePriority.Highest, new UserOnPacketCheckPasswordFlippedPlug(
            Logger,
            options,
            ctx
        ));

        // Auto-login: after world list is sent, wait for client to process, then auto-select world
        ctx.Pipelines.UserOnPacketWorldRequest.Add(PipelinePriority.PostNormal, new UserOnPacketCheckPasswordAutoSelectWorldPlug(
            Logger,
            options,
            ctx,
            _diagnostics,
            _memoryContext,
            tracker
        ));

        // Auto-login: after successful world selection, auto-select character and enter game
        ctx.Pipelines.UserOnPacketSelectWorld.Add(PipelinePriority.PostNormal, new UserOnPacketSelectWorldAutoSelectCharacterPlug(
            Logger,
            options,
            ctx,
            ctx.Repositories.Character,
            _memoryContext,
            tracker
        ));

        return Task.CompletedTask;
    }

    public Task OnStop()
    {
        _memoryContext.Dispose();
        return Task.CompletedTask;
    }
}
