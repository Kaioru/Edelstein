using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server;

public class SystemHost<TStageSystem, TStageSystemUser>(
    ILogger<SystemHost<TStageSystem, TStageSystemUser>> logger,
    IStageSystem<TStageSystem, TStageSystemUser> system,
    IStageSystemInfo info,
    TransportVersion version
) : IHostedService
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>
{
    private ITransportContext? Context { get; set; }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var acceptor = new NettyTransportAcceptor<TStageSystemUser>(
            version,
            system,
            system
        );
        
        Context = await acceptor.Accept(info.Host, info.Port);
        
        logger.LogSystemHostStarted(
            info.ID,
            version.Major, version.Patch, version.Locale,
            info.Host, info.Port
        );
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogSystemHostStopping(info.ID);
        
        if (Context != null)
            await Context.Close();
        
        logger.LogSystemHostStopped(info.ID);
    }
}
