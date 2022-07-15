using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Pipelines;
using Edelstein.Daemon.Server;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        services.Configure<ProgramConfig>(ctx.Configuration.GetSection("Host"));

        services.Scan(s => s
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IPacketHandler<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IPipelineAction<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

        services.AddSingleton(typeof(IPacketHandlerManager<>), typeof(PacketHandlerManager<>));
        services.AddSingleton(typeof(IPipeline<>), typeof(Pipeline<>));

        services.AddSingleton(services);
        services.AddHostedService<ProgramHost>();
    })
    .RunConsoleAsync();
