using Autofac;
using Edelstein.Application.Server.Configs;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Application.Server;

public class ProgramHost : IHostedService
{
    private readonly ProgramConfig _config;
    private readonly ILifetimeScope _scope;

    public ProgramHost(ProgramConfig config, ILifetimeScope scope)
    {
        _config = config;
        _scope = scope;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var stages = new List<ProgramConfigStage>();
        
        stages.AddRange(_config.LoginStages);

        foreach (var stage in stages)
        {
            using var stageScope = _scope.BeginLifetimeScope(b =>
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
                    .SingleInstance();;
                
                switch (stage)
                {
                    case ILoginStageOptions options:
                        b.RegisterInstance(options).SingleInstance();
                        b.RegisterType<LoginContext>().SingleInstance();
                        b.RegisterType<LoginContextPipelines>().SingleInstance();
                        b.RegisterType<LoginContextServices>().SingleInstance();

                        b.RegisterType<LoginStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.RegisterInstance(new LoginStage(stage.ID)).As<ILoginStage>().SingleInstance();
                        break;
                }
            });

            stageScope.Resolve<ITransportAcceptor>().Accept(stage.Host, stage.Port);
        }

        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
