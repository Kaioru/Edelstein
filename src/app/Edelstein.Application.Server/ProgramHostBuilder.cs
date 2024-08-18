using System.Reflection;
using Edelstein.Application.Server.Bindings;
using Edelstein.Common.Database;
using Edelstein.Common.Database.Pgsql;
using Edelstein.Common.Database.Sqlite;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Plugin;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Utilities.Bootstrap;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Templates;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Edelstein.Application.Server;

internal static class ProgramHostBuilder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.Configure<ProgramHostConfig>(builder.Configuration.GetSection("Host"));

        builder.Services.AddMapster();
        builder.Services.AddPooledDbContextFactory<GameDbContext>(options =>
        {
            switch (builder.Configuration["DatabaseProvider"])
            {
                case "Sqlite":
                    options.UseSqlite(builder.Configuration.GetConnectionString(SqliteDbContextFactory.Sqlite.Key));
                    break;
                case "Pgsql":
                    options.UseNpgsql(builder.Configuration.GetConnectionString(PgsqlDbContextFactory.Pgsql.Key));
                    break;
            }
        });
        builder.Services.Scan(scan => scan
            .FromAssemblyOf<GameDbContext>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueriedRepository<,>))).AsImplementedInterfaces()
            .WithSingletonLifetime());

        // TODO gRPC
        builder.Services.AddSingleton<IAuthService, AuthService>();
        
        builder.Services.AddSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));
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
            .WithScopedLifetime());

        return builder;
    }
}
