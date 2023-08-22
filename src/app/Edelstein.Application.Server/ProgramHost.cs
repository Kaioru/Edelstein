using System.Reflection;
using Autofac;
using Edelstein.Application.Server.Bootstraps;
using Edelstein.Application.Server.Configs;
using Edelstein.Common.Database;
using Edelstein.Common.Gameplay;
using Edelstein.Common.Gameplay.Game;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.EntityFrameworkCore;
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
        var programScope = _scope.BeginLifetimeScope(b =>
        {
            var assemblies = new List<Assembly> { Assembly.GetAssembly(typeof(AbstractStage<>))! };
            
            if (_config.LoginStages.Count > 0)
                assemblies.Add(Assembly.GetAssembly(typeof(LoginStage))!);
            
            b
                .RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(ITemplateLoader)))
                .AsImplementedInterfaces()
                .SingleInstance();
        });
        var stages = new List<ProgramConfigStage>();
        
        stages.AddRange(_config.LoginStages);
        stages.AddRange(_config.GameStages);
        
        foreach (var stage in stages)
        {
            await using var stageScope = programScope.BeginLifetimeScope(b =>
            {
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
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LoginStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LoginStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();
                        
                        b
                            .RegisterInstance(options)
                            .As<ILoginStageOptions>()
                            .As<ProgramConfigStageLogin>()
                            .SingleInstance();
                        b.RegisterType<LoginContext>().SingleInstance();
                        b.RegisterType<LoginContextManagers>().SingleInstance();
                        b.RegisterType<LoginContextServices>().SingleInstance();
                        b.RegisterType<LoginContextRepositories>().SingleInstance();
                        b.RegisterType<LoginContextTemplates>().SingleInstance();
                        b.RegisterType<LoginContextPipelines>().SingleInstance();

                        b.RegisterType<LoginStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.RegisterInstance(new LoginStage(stage.ID))
                            .As<IStage<ILoginStageUser>>()
                            .As<ILoginStage>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageLogin>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                    case IGameStageOptions options:
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();
                        
                        b
                            .RegisterInstance(options)
                            .As<IGameStageOptions>()
                            .As<ProgramConfigStageGame>()
                            .SingleInstance();
                        b.RegisterType<GameContext>().SingleInstance();
                        b.RegisterType<GameContextManagers>().SingleInstance();
                        b.RegisterType<GameContextServices>().SingleInstance();
                        b.RegisterType<GameContextRepositories>().SingleInstance();
                        b.RegisterType<GameContextTemplates>().SingleInstance();
                        b.RegisterType<GameContextPipelines>().SingleInstance();

                        b.RegisterType<GameStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.RegisterInstance(new GameStage(stage.ID))
                            .As<IStage<IGameStageUser>>()
                            .As<IGameStage>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageGame>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                }
            });
            
            foreach (var bootstrap in stageScope.Resolve<IEnumerable<IBootstrap>>())
                _bootstraps.Add(bootstrap);
        }
        
        _bootstraps.Add(new InitDatabaseBootstrap(
            programScope.Resolve<ILogger<InitDatabaseBootstrap>>(),
            programScope.Resolve<IDbContextFactory<GameplayDbContext>>(),
            programScope.Resolve<IDbContextFactory<AuthDbContext>>(),
            programScope.Resolve<IDbContextFactory<ServerDbContext>>(),
            _config
        ));
        _bootstraps.Add(new InitTickerBootstrap(programScope.Resolve<ITickerManager>()));
        _bootstraps.Add(new LoadTemplateBootstrap(
            programScope.Resolve<ILogger<LoadTemplateBootstrap>>(), 
            programScope.Resolve<IEnumerable<ITemplateLoader>>())
        );
        
        foreach (var bootstrap in _bootstraps.OrderBy(b => b.Priority))
            await bootstrap.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var bootstrap in _bootstraps)
            await bootstrap.Stop();
    }
}
