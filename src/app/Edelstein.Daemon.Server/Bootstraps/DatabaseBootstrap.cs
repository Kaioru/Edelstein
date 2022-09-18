using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Database;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class DatabaseBootstrap : IBootstrap
{
    private readonly IDbContextFactory<AuthDbContext> _authContextFactory;

    private readonly IDbContextFactory<GameplayDbContext> _gameContextFactory;
    private readonly IDbContextFactory<ServerDbContext> _serverContextFactory;
    private readonly ILogger _logger;

    public DatabaseBootstrap(
        ILogger<DatabaseBootstrap> logger,
        IDbContextFactory<GameplayDbContext> gameContextFactory,
        IDbContextFactory<AuthDbContext> authContextFactory,
        IDbContextFactory<ServerDbContext> serverContextFactory
    )
    {
        _logger = logger;
        _gameContextFactory = gameContextFactory;
        _authContextFactory = authContextFactory;
        _serverContextFactory = serverContextFactory;
    }

    public async Task Start()
    {
        await using var _gameContext = await _gameContextFactory.CreateDbContextAsync();
        await using var _authContext = await _authContextFactory.CreateDbContextAsync();
        await using var _serverContext = await _serverContextFactory.CreateDbContextAsync();

        if ((await _gameContext.Database.GetPendingMigrationsAsync()).Any() ||
            (await _authContext.Database.GetPendingMigrationsAsync()).Any() ||
            (await _serverContext.Database.GetPendingMigrationsAsync()).Any())
            _logger.LogWarning("There are database migrations currently pending, this might cause unwanted behavior");
    }

    public Task Stop() => Task.CompletedTask;
}
