using Autofac;
using Autofac.Extensions.DependencyInjection;
using Edelstein.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json", true);
        builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
        builder.AddCommandLine(args);
    })
    .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices((ctx, services) =>
    {
        services.AddPooledDbContextFactory<GameplayDbContext>(o 
            => o.UseNpgsql(ctx.Configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey)));
    })
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        var container = builder.Build();
        
        // TODO: Scoped for each stage
        // Register Stage
        // Register Startup Tasks
    })
    .RunConsoleAsync();
