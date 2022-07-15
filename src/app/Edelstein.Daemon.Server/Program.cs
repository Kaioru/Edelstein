using Edelstein.Daemon.Server;
using Edelstein.Daemon.Server.Configs;
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
        services.AddSingleton(services);

        services.Configure<ProgramConfig>(ctx.Configuration.GetSection("Host"));
        services.AddSingleton<ProgramHostContext>();
        services.AddHostedService<ProgramHost>();
    })
    .RunConsoleAsync();
