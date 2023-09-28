using Edelstein.Application.Server.Configs;
using Edelstein.Common.Database;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Services.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class InitDatabaseBootstrap : IBootstrap
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<AuthDbContext> _authContext;
    private readonly IDbContextFactory<ServerDbContext> _serverContext;
    private readonly IDbContextFactory<GameplayDbContext> _gameContext;
    private readonly IDbContextFactory<SocialDbContext> _socialContext;
    private readonly ProgramConfig _config;

    public InitDatabaseBootstrap(
        ILogger<InitDatabaseBootstrap> logger,
        IDbContextFactory<AuthDbContext> authContext,
        IDbContextFactory<ServerDbContext> serverContext,
        IDbContextFactory<GameplayDbContext> gameContext,
        IDbContextFactory<SocialDbContext> socialContext,
        ProgramConfig config
    )
    {
        _logger = logger;
        _authContext = authContext;
        _serverContext = serverContext;
        _gameContext = gameContext;
        _socialContext = socialContext;
        _config = config;
    }

    public int Priority => BootstrapPriority.Init;

    public async Task Start()
    {
        if (!_config.MigrateDatabaseOnInit) return;

        await using var db0 = await _authContext.CreateDbContextAsync();
        await using var db1 = await _serverContext.CreateDbContextAsync();
        await using var db2 = await _gameContext.CreateDbContextAsync();
        await using var db3 = await _socialContext.CreateDbContextAsync();

        await db0.Database.MigrateAsync();
        await db1.Database.MigrateAsync();
        await db2.Database.MigrateAsync();
        await db3.Database.MigrateAsync();

        _logger.LogWarning("Migrated databases, this option is not recommended on production!");
    }

    public Task Stop() => Task.CompletedTask;
}
