using Autofac;
using Edelstein.Application.Server.Bootstraps;
using Edelstein.Application.Server.Configs;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server;

public class ProgramHost : IHostedService
{
    private readonly ICollection<IBootstrap> _bootstraps;
    private readonly ProgramConfig _config;
    private readonly ILifetimeScope _scope;

    public ProgramHost(ProgramConfig config, ILifetimeScope scope)
    {
        _bootstraps = new HashSet<IBootstrap>();
        _config = config;
        _scope = scope;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var stages = new List<ProgramConfigStage>();
        
        stages.AddRange(_config.LoginStages);

        foreach (var stage in stages)
        {
            await using var stageScope = _scope.BeginLifetimeScope(b =>
            {
                b
                    .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                    .Where(t => t.IsClass)
                    .AsClosedTypesOf(typeof(IPacketHandler<>))
                    .SingleInstance();
                b
                    .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                    .Where(t => t.IsClass)
                    .AsClosedTypesOf(typeof(IPipelinePlug<>))
                    .SingleInstance();
                
                b.RegisterGeneric(typeof(PacketHandlerManager<>)).As(typeof(IPacketHandlerManager<>)).SingleInstance();
                b.RegisterGeneric(typeof(Pipeline<>)).As(typeof(IPipeline<>)).SingleInstance();
                
                b
                    .Register(c => new NettyTransportAcceptor(
                        c.Resolve<IAdapterInitializer>(), 
                        new TransportVersion(stage.Version, stage.Patch, stage.Locale))
                    )
                    .As<ITransportAcceptor>()
                    .SingleInstance();

                b.RegisterType<StartStageBootstrap>().As<IBootstrap>().SingleInstance();
                b.Register(c => new StartServerBootstrap(
                    c.Resolve<ILogger<StartServerBootstrap>>(),
                    c.Resolve<ITickerManager>(),
                    c.Resolve<ITransportAcceptor>(),
                    stage
                )).As<IBootstrap>().SingleInstance();
                
                switch (stage)
                {
                    case ILoginStageOptions options:
                        b
                            .RegisterInstance(options)
                            .As<ILoginStageOptions>()
                            .As<ProgramConfigStageLogin>()
                            .SingleInstance();
                        b.RegisterType<LoginContext>().SingleInstance();
                        b.RegisterType<LoginContextPipelines>().SingleInstance();
                        b.RegisterType<LoginContextServices>().SingleInstance();

                        b.RegisterType<LoginStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.RegisterInstance(new LoginStage(stage.ID))
                            .As<IStage<ILoginStageUser>>()
                            .As<ILoginStage>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageLogin>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                }
            });
            
            foreach (var bootstrap in stageScope.Resolve<IEnumerable<IBootstrap>>())
                _bootstraps.Add(bootstrap);
        }
        
        _bootstraps.Add(new InitTickerBootstrap(_scope.Resolve<ITickerManager>()));
        
        foreach (var bootstrap in _bootstraps.OrderBy(b => b.Priority))
            await bootstrap.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var bootstrap in _bootstraps)
            await bootstrap.Stop();
    }
}
