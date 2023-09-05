using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class ServerDbContext : DbContext
{
    public const string ConnectionStringKey = "Server";

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    public DbSet<ServerEntity> Servers { get; set; }
    public DbSet<ServerLoginEntity> LoginServers { get; set; }
    public DbSet<ServerGameEntity> GameServers { get; set; }
    public DbSet<ServerShopEntity> ShopServers { get; set; }

    public DbSet<SessionEntity> Sessions { get; set; }
    public DbSet<MigrationEntity> Migrations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(ServerDbContext).Assembly);
}
