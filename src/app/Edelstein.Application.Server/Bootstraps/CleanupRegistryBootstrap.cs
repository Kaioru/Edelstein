using Edelstein.Application.Server.Configs;
using Edelstein.Common.Services.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class CleanupRegistryBootstrap : IBootstrap
{
    private readonly ILogger<CleanupRegistryBootstrap> _logger;
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;
    private readonly ProgramConfig _config;

    public CleanupRegistryBootstrap(
        ILogger<CleanupRegistryBootstrap> logger,
        IDbContextFactory<ServerDbContext> dbFactory,
        ProgramConfig config)
    {
        _logger = logger;
        _dbFactory = dbFactory;
        _config = config;
    }

    public int Priority => BootstrapPriority.Init - 1;

    public async Task Start()
    {
        if (!_config.CleanupRegistryOnInit) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        var now = DateTime.UtcNow;
        var expired = await db.Servers
            .Where(s => s.DateExpire < now)
            .ToListAsync();

        if (expired.Count != 0)
        {
            db.Servers.RemoveRange(expired);
            await db.SaveChangesAsync();
            _logger.LogInformation("Cleaned up {Count} expired registry entries", expired.Count);
        }
    }

    public Task Stop() => Task.CompletedTask;
}
