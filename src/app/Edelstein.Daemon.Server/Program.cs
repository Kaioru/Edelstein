using System.Collections.Immutable;
using Edelstein.Common.Data.NX;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Database;
using Edelstein.Common.Gameplay.Database.Repositories;
using Edelstein.Common.Gameplay.Database.Serializers;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Util.Pipelines;
using Edelstein.Common.Util.Templates;
using Edelstein.Common.Util.Tickers;
using Edelstein.Daemon.Server;
using Edelstein.Daemon.Server.Bootstraps;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;
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

        services.AddSingleton<ISerializer, BinarySerializer>();

        services.AddSingleton<IAccountRepository, AccountRepository>();
        services.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IServerService, ServerService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IMigrationService, MigrationService>();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<IDataManager>(new NXDataManager(ctx.Configuration.GetSection("Data")["Directory"]));
        services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<ITickerManager, TickerManager>();
        services.AddSingleton<IBootstrap, TickerStartBootstrap>();
    })
    .ConfigureServices((ctx, services) =>
    {
        var config = ctx.Configuration.GetSection("Host").Get<ProgramConfig>();
        var stages = new List<AbstractProgramConfigStage>();

        services.AddSingleton(config);
        stages.AddRange(config.LoginStages);

        foreach (var stage in stages)
            services.AddSingleton<IBootstrap>(p =>
            {
                var scope = new ServiceCollection { services };

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

                switch (stage)
                {
                    case ILoginContextOptions options:
                        scope.Scan(s => s
                            .FromAssemblyOf<LoginStage>()
                            .AddClasses(c => c.AssignableTo<ITemplateLoader>())
                            .AsImplementedInterfaces()
                            .WithSingletonLifetime()
                        );

                        scope.AddSingleton(options);
                        scope.AddSingleton<ILoginContext, LoginContext>();
                        scope.AddSingleton<ILoginContextPipelines, LoginContextPipelines>();
                        scope.AddSingleton<ILoginContextServices, LoginContextServices>();
                        scope.AddSingleton<ILoginContextTemplates, LoginContextTemplates>();
                        scope.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();
                        scope.AddSingleton<ILoginStage, LoginStage>();
                        break;
                }

                var provider = scope.BuildServiceProvider();
                var adapter = provider.GetRequiredService<IAdapterInitializer>();
                var acceptor = new NettyTransportAcceptor(adapter, stage.Version, stage.Patch, stage.Locale);
                var loaders = provider.GetServices<ITemplateLoader>();

                return new ServerStartBootstrap(
                    p.GetRequiredService<ILogger<ServerStartBootstrap>>(),
                    acceptor,
                    stage,
                    loaders.ToImmutableList()
                );
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
    })
    .ConfigureServices((ctx, services) => { services.AddHostedService<ProgramHost>(); })
    .RunConsoleAsync();
