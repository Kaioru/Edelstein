using System;
using System.Reflection;
using Duey.Abstractions;
using Duey.Provider.NX;
using Duey.Provider.WZ;
using Edelstein.Application.Server.Bindings;
using Edelstein.Application.Server.Services;
using Edelstein.Common.Database;
using Edelstein.Common.Database.Pgsql;
using Edelstein.Common.Database.Sqlite;
using Edelstein.Common.Gameplay.Game;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Plugin;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Migration;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Services.Session;
using Edelstein.Common.Utilities;
using Edelstein.Common.Utilities.Bootstrap;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Common.Utilities.Tickers;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using SqliteExceptionProcessorExtensions = EntityFramework.Exceptions.Sqlite.ExceptionProcessorExtensions;
using PgSqlExceptionProcessorExtensions = EntityFramework.Exceptions.PostgreSQL.ExceptionProcessorExtensions;

namespace Edelstein.Application.Server;

internal static class ProgramHostBuilder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));
        builder.Services.Configure<ProgramHostConfig>(builder.Configuration.GetSection("Host"));

        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<ITicker>(p =>
        {
            var options = p.GetRequiredService<IOptions<ProgramHostConfig>>().Value;
            
            return new TickerPool(
                options.TargetTicksPerSecond, 
                options.TargetTicksPoolCount
            );
        });
        builder.Services.Scan(scan => scan
            .FromAssemblyDependencies(Assembly.GetEntryAssembly()!)
            .AddClasses(classes => classes.AssignableTo<ITickerAction>()).AsImplementedInterfaces()
            .WithSingletonLifetime());
        builder.Services.AddHostedService<TickerHostService>();

        
        switch (builder.Configuration.GetSection("Data")["Type"])
        {
            case "NX":
                builder.Services.AddSingleton<IDataNamespace>(
                    new NXNamespace(builder.Configuration.GetSection("Data")["Directory"] ?? throw new InvalidOperationException())
                );
                break;
            case "WZ":
                builder.Services.AddSingleton<IDataNamespace>(new WZNamespace(
                    builder.Configuration.GetSection("Data")["Directory"] ?? throw new InvalidOperationException(),
                    builder.Configuration.GetSection("Data")["Key"] ?? throw new InvalidOperationException()));
                break;
        }
        
        builder.Services.AddAutoMapper(typeof(Program), typeof(GameDbContext));
        builder.Services.AddPooledDbContextFactory<GameDbContext>(options =>
        {
            switch (builder.Configuration["DatabaseProvider"])
            {
                case "Sqlite":
                    SqliteExceptionProcessorExtensions.UseExceptionProcessor(options
                        .UseSqlite(builder.Configuration.GetConnectionString(SqliteDbContextFactory.Sqlite.Key))
                    );
                    break;
                case "Pgsql":
                    PgSqlExceptionProcessorExtensions.UseExceptionProcessor(options
                        .UseNpgsql(builder.Configuration.GetConnectionString(PgsqlDbContextFactory.Pgsql.Key))
                    );
                    break;
            }
        });
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<GameDbContext>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueriedRepository<,>))).AsImplementedInterfaces()
            .WithSingletonLifetime());

        // TODO gRPC
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IServerService, ServerService>();
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<IMigrationService, MigrationService>();
        
        builder.Services.AddSingleton(typeof(ITemplateManagerContext<>), typeof(TemplateManagerContext<>));
        builder.Services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));
        
        builder.Services.Scan(scan => scan
            .FromAssemblyDependencies(Assembly.GetEntryAssembly()!)
            .AddClasses(classes => classes.AssignableTo<IBootLoader>()).AsImplementedInterfaces()
            .WithSingletonLifetime());
        
        builder.Services.AddScoped(typeof(IPluginManager<>), typeof(PluginManager<>));
        
        builder.Services.AddScoped(typeof(IPacketHandlerManager<,>), typeof(PacketHandlerManager<,>));
        builder.Services.AddScoped(typeof(IPipeline<>), typeof(Pipeline<>));
        builder.Services.Scan(scan => scan
            .FromAssemblyDependencies(Assembly.GetEntryAssembly()!)
            .AddClasses(classes => classes.AssignableTo(typeof(IPacketHandlerManagerEntry<,>))).AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(IPipe<>))).AsImplementedInterfaces()
            .WithScopedLifetime());
        
        builder.Services.Scan(scan => scan
            .FromAssemblyDependencies(Assembly.GetEntryAssembly()!)
            .AddClasses(classes => classes.InExactNamespaceOf<LoginContext>()).AsSelf()
            .AddClasses(classes => classes.InExactNamespaceOf<GameContext>()).AsSelf()
            .WithScopedLifetime());

        builder.Services.AddScoped<IFieldManager, FieldManager>();

        return builder;
    }
}
