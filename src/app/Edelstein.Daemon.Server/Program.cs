using Edelstein.Common.Data.NX;
using Edelstein.Common.Gameplay.Database;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Util.Templates;
using Edelstein.Daemon.Server;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Util.Templates;
using Microsoft.EntityFrameworkCore;
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

        services.AddPooledDbContextFactory<AuthDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(AuthDbContext.ConnectionStringKey)));
        services.AddPooledDbContextFactory<GameplayDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey)));

        services.AddSingleton<IDataManager>(new NXDataManager(ctx.Configuration.GetSection("Data")["Directory"]));

        services.Scan(s => s
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo<ITemplateLoader>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );
        services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));

        services.AddSingleton<ILoginContextTemplates, LoginContextTemplates>();

        services.Configure<ProgramConfig>(ctx.Configuration.GetSection("Host"));
        services.AddSingleton<ProgramContext>();
        services.AddHostedService<ProgramHost>();
    })
    .RunConsoleAsync();
