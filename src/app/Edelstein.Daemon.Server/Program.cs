using Edelstein.Common.Data.NX;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Database;
using Edelstein.Common.Gameplay.Database.Repositories;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Game.Contexts;
using Edelstein.Common.Gameplay.Stages.Game.Continents;
using Edelstein.Common.Gameplay.Stages.Game.Conversations;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Plugin;
using Edelstein.Common.Scripting.MoonSharp;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Util.Commands;
using Edelstein.Common.Util.Events;
using Edelstein.Common.Util.Pipelines;
using Edelstein.Common.Util.Serializers;
using Edelstein.Common.Util.Templates;
using Edelstein.Common.Util.Tickers;
using Edelstein.Daemon.Server;
using Edelstein.Daemon.Server.Bootstraps;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Scripting;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Commands;
using Edelstein.Protocol.Util.Events;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
using Edelstein.Protocol.Util.Tickers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json", true);
        builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
        builder.AddCommandLine(args);
    })
    .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices((ctx, services) =>
    {
        services.AddPooledDbContextFactory<AuthDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(AuthDbContext.ConnectionStringKey)));
        services.AddPooledDbContextFactory<ServerDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(ServerDbContext.ConnectionStringKey)));
        services.AddPooledDbContextFactory<GameplayDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey)));
        services.AddSingleton<IBootstrap, DatabaseBootstrap>();

        services.AddSingleton<ISerializer, BinarySerializer>();

        services.AddSingleton<IAccountRepository, AccountRepository>();
        services.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
        services.AddSingleton<ICharacterRepository, CharacterRepository>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IServerService, ServerService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IMigrationService, MigrationService>();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<ITickerManager, TickerManager>(p => new TickerManager(
            p.GetRequiredService<ILogger<TickerManager>>(),
            p.GetRequiredService<ProgramConfig>().TicksPerSecond
        ));
        services.AddSingleton<IDataManager>(new NXDataManager(ctx.Configuration.GetSection("Data")["Directory"]));
        services.AddSingleton<IScriptEngine>(new LuaScriptEngine(ctx.Configuration.GetSection("Scripts")["Directory"]));
        services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));
        services.AddSingleton(typeof(IPluginManager<>), typeof(PluginManager<>));
    })
    .ConfigureServices((ctx, services) =>
    {
        var config = ctx.Configuration.GetSection("Host").Get<ProgramConfig>();
        var stages = new List<AbstractProgramConfigStage>();

        services.AddSingleton(config);
        stages.AddRange(config.LoginStages);
        stages.AddRange(config.GameStages);

        foreach (var stage in stages)
            services.AddSingleton<IBootstrap>(p =>
            {
                var scope = new ServiceCollection { services };

                scope.AddSingleton(p.GetRequiredService<ITickerManager>());
                scope.Scan(s => s
                    .FromApplicationDependencies()
                    .AddClasses(c => c.AssignableTo(typeof(IPacketHandler<>)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(c => c.AssignableTo(typeof(IPipelinePlug<>)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()
                );
                scope.AddSingleton(typeof(IPacketHandlerManager<>), typeof(PacketHandlerManager<>));
                scope.AddSingleton(typeof(IPipeline<>), typeof(Pipeline<>));
                scope.AddSingleton(typeof(IEvent<>), typeof(Event<>));

                scope.AddSingleton<ITransportAcceptor>(p => new NettyTransportAcceptor(
                    p.GetRequiredService<IAdapterInitializer>(),
                    stage.Version, stage.Patch, stage.Locale
                ));

                switch (stage)
                {
                    case ILoginContextOptions options:
                        scope.Scan(s => s
                            .FromAssembliesOf(typeof(AbstractStage<>), typeof(LoginStage))
                            .AddClasses(c => c.AssignableTo<ITemplateLoader>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                            .FromAssembliesOf(typeof(AbstractStage<>), typeof(LoginStage))
                            .AddClasses(c => c.AssignableTo<ITickable>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                        );

                        scope.AddSingleton(options);
                        scope.AddSingleton<ILoginContext, LoginContext>();
                        scope.AddSingleton<ILoginContextEvents, LoginContextEvents>();
                        scope.AddSingleton<ILoginContextPipelines, LoginContextPipelines>();
                        scope.AddSingleton<ILoginContextServices, LoginContextServices>();
                        scope.AddSingleton<ILoginContextManagers, LoginContextManagers>();
                        scope.AddSingleton<ILoginContextTemplates, LoginContextTemplates>();
                        scope.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();
                        scope.AddSingleton<ILoginStage, LoginStage>();
                        break;
                    case IGameContextOptions options:
                        scope.Scan(s => s
                            .FromAssembliesOf(typeof(AbstractStage<>), typeof(GameStage))
                            .AddClasses(c => c.AssignableTo<ITemplateLoader>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                            .FromAssembliesOf(typeof(AbstractStage<>), typeof(GameStage))
                            .AddClasses(c => c.AssignableTo<ITickable>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                        );

                        scope.AddSingleton<IFieldManager, FieldManager>();
                        scope.AddSingleton<IContiMoveManager, ContiMoveManager>();
                        scope.AddSingleton(typeof(ICommandManager<>), typeof(CommandManager<>));
                        scope.AddSingleton<IConversationManager, ScriptedConversationManager>();

                        scope.AddSingleton(options);
                        scope.AddSingleton<IGameContext, GameContext>();
                        scope.AddSingleton<IGameContextEvents, GameContextEvents>();
                        scope.AddSingleton<IGameContextPipelines, GameContextPipelines>();
                        scope.AddSingleton<IGameContextServices, GameContextServices>();
                        scope.AddSingleton<IGameContextManagers, GameContextManagers>();
                        scope.AddSingleton<IGameContextTemplates, GameContextTemplates>();
                        scope.AddSingleton<IAdapterInitializer, GameStageUserInitializer>();
                        scope.AddSingleton<IGameStage, GameStage>();
                        break;
                }

                scope.AddSingleton(stage);
                scope.AddSingleton(typeof(ServerStartBootstrap<,,>));

                var provider = scope.BuildServiceProvider();

                return stage switch
                {
                    ILoginContextOptions => provider.GetRequiredService<ServerStartBootstrap<
                        ILoginStage,
                        ILoginStageUser,
                        ILoginContext
                    >>(),
                    IGameContextOptions => provider.GetRequiredService<ServerStartBootstrap<
                        IGameStage,
                        IGameStageUser,
                        IGameContext
                    >>(),
                    _ => new ServerVoidBootstrap()
                };
            });

        foreach (var login in config.LoginStages)
            services.AddSingleton<IBootstrap>(p =>
                new ServerUpdateBootstrap<ProgramConfigStageLogin>(
                    p.GetRequiredService<ILogger<ServerUpdateBootstrap<ProgramConfigStageLogin>>>(),
                    login,
                    p.GetRequiredService<IServerService>(),
                    p.GetRequiredService<ITickerManager>()
                )
            );
        foreach (var game in config.GameStages)
            services.AddSingleton<IBootstrap>(p =>
                new ServerUpdateBootstrap<ProgramConfigStageGame>(
                    p.GetRequiredService<ILogger<ServerUpdateBootstrap<ProgramConfigStageGame>>>(),
                    game,
                    p.GetRequiredService<IServerService>(),
                    p.GetRequiredService<ITickerManager>()
                )
            );
    })
    .ConfigureServices((ctx, services) => { services.AddSingleton<IBootstrap, TickerStartBootstrap>(); })
    .ConfigureServices((ctx, services) => { services.AddHostedService<ProgramHost>(); })
    .RunConsoleAsync();
