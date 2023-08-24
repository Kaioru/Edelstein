using Edelstein.Application.Server.Configs;
using Edelstein.Common.Database;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class InitDatabaseBootstrap : IBootstrap
{
    private readonly IDbContextFactory<AuthDbContext> _authContext;
    private readonly ProgramConfig _config;
    private readonly IDbContextFactory<GameplayDbContext> _gameContext;
    private readonly ILogger _logger;
    private readonly IDbContextFactory<ServerDbContext> _serverContext;

    public InitDatabaseBootstrap(
        ILogger<InitDatabaseBootstrap> logger,
        IDbContextFactory<GameplayDbContext> gameContext,
        IDbContextFactory<AuthDbContext> authContext,
        IDbContextFactory<ServerDbContext> serverContext,
        ProgramConfig config
    )
    {
        _logger = logger;
        _gameContext = gameContext;
        _authContext = authContext;
        _serverContext = serverContext;
        _config = config;
    }

    public int Priority => BootstrapPriority.Init;

    public async Task Start()
    {
        if (!_config.MigrateDatabaseOnInit) return;

        await using var db0 = await _gameContext.CreateDbContextAsync();
        await using var db1 = await _authContext.CreateDbContextAsync();
        await using var db2 = await _serverContext.CreateDbContextAsync();

        await db0.Database.MigrateAsync();
        await db1.Database.MigrateAsync();
        await db2.Database.MigrateAsync();

        _logger.LogWarning("Migrated databases, this option is not recommended on production!");
    }
    public Task Stop() => Task.CompletedTask;
}
