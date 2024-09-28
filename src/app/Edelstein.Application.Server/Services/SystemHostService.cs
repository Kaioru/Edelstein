using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Application.Server.Bindings;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Application.Server.Services;

public class SystemHostService<TStageSystem, TStageSystemUser, TContext>(
    ILogger<SystemHostService<TStageSystem, TStageSystemUser, TContext>> logger,
    IOptions<ProgramHostConfig> config,
    IStageSystem<TStageSystem, TStageSystemUser> system,
    IServerInfo info,
    TransportVersion version,
    IPluginManager<TContext> plugins,
    TContext context,
    IPipeline<SystemOnStart<TStageSystem, TStageSystemUser>> onStart,
    IPipeline<SystemOnStop<TStageSystem, TStageSystemUser>> onStop
) : IHostedService
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>
{
    private ITransportContext? Context { get; set; }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await onStart.Process(new SystemOnStart<TStageSystem, TStageSystemUser>
        {
            System = system
        });
        
        await plugins.LoadFromDirectory(Path.GetFullPath(config.Value.PluginDirectory));
        await plugins.InvokeInit(context);
        await plugins.InvokeStart(context);

        Context = await new NettyTransportAcceptor<TStageSystemUser>(
            version,
            system,
            system
        ).Accept(info.Host, info.Port);
        
        logger.LogSystemHostServiceStarted(
            info.ID,
            version.Major, version.Patch, version.Locale,
            info.Host, info.Port
        );
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogSystemHostServiceStopping(info.ID);
        
        await onStop.Process(new SystemOnStop<TStageSystem, TStageSystemUser>
        {
            System = system
        });
        
        if (Context != null)
            await Context.Close();

        logger.LogSystemHostServiceStopped(info.ID);
        
        await plugins.InvokeStop();
    }
}
