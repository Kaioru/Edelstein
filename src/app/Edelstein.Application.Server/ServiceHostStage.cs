using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server;

public class ServiceHostStage<TStageUser, TStageSystem>(
    ILogger<ServiceHostStage<TStageUser, TStageSystem>> logger,
    TransportVersion version,
    IServerEntry settings,
    IStageSystem<TStageUser, TStageSystem> system
) : IHostedService
    where TStageUser : class, IStageUser<TStageUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    private ITransportContext? Context { get; set; }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var acceptor = new NettyTransportAcceptor<TStageUser>(
            version,
            system,
            system
        );

        Context = await acceptor.Accept(settings.Host, settings.Port);
        logger.LogInformation(
            "{ID} socket acceptor for v{Version}.{Patch} (Locale {Locale}) bound at {Host}:{Port}",
            settings.ID,
            version.Major, version.Patch, version.Locale,
            settings.Host, settings.Port
        );
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "{ID} socket acceptor shutting down, this may take awhile..",
            settings.ID
        );
        
        if (Context != null)
            await Context.Close();
        
        logger.LogInformation(
            "{ID} socket acceptor finished shutting down",
            settings.ID
        );
    }
}
