﻿using Autofac.Extensions.DependencyInjection;
using Duey.Abstractions;
using Duey.Provider.NX;
using Duey.Provider.WZ;
using Edelstein.Application.Server;
using Edelstein.Application.Server.Configs;
using Edelstein.Common.Database;
using Edelstein.Common.Database.Repositories;
using Edelstein.Common.Scripting.Lua;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Services.Social;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Common.Utilities.Tickers;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Scripting;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;
using Foundatio.Messaging;
using Foundatio.Serializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json", true);
        builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
        builder.AddEnvironmentVariables();
        builder.AddCommandLine(args);
    })
    .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton(ctx.Configuration.GetSection("Host").Get<ProgramConfig>()!);

        services.AddAutoMapper(typeof(AuthDbContext), typeof(ServerDbContext), typeof(GameplayDbContext));
        services.AddDbContextFactory<AuthDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(AuthDbContext.ConnectionStringKey)));
        services.AddDbContextFactory<ServerDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(ServerDbContext.ConnectionStringKey)));
        services.AddDbContextFactory<GameplayDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey)));
        services.AddDbContextFactory<SocialDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString(SocialDbContext.ConnectionStringKey)));

        services.AddSingleton<IMessageBus>(new InMemoryMessageBus(
            new InMemoryMessageBusOptions
            {
                Serializer = new JsonNetSerializer(new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All})
            }));

        services.AddSingleton<IAccountRepository, AccountRepository>();
        services.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
        services.AddSingleton<ICharacterRepository, CharacterRepository>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IServerService, ServerService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IMigrationService, MigrationService>();
        
        services.AddSingleton<IFriendService, FriendService>();
        services.AddSingleton<IPartyService, PartyService>();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<ITickerManager, TickerManager>(p => new TickerManager(
            p.GetRequiredService<ILogger<TickerManager>>(),
            p.GetRequiredService<ProgramConfig>().TicksPerSecond
        ));

        switch (ctx.Configuration.GetSection("Data")["Type"])
        {
            case "NX":
                services.AddSingleton<IDataNamespace>(
                    new NXNamespace(ctx.Configuration.GetSection("Data")["Directory"] ?? throw new InvalidOperationException())
                );
                break;
            case "WZ":
                services.AddSingleton<IDataNamespace>(new WZNamespace(
                    ctx.Configuration.GetSection("Data")["Directory"] ?? throw new InvalidOperationException(),
                    ctx.Configuration.GetSection("Data")["Key"] ?? throw new InvalidOperationException()));
                break;
        }

        services.AddSingleton<IScriptEngine>(new LuaScriptEngine(ctx.Configuration.GetSection("Scripts")["Directory"] ?? throw new InvalidOperationException()));
        services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));
    })
    .ConfigureServices((ctx, services) => { services.AddHostedService<ProgramHost>(); })
    .RunConsoleAsync();
