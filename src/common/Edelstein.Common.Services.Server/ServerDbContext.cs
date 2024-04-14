using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class ServerDbContext(DbContextOptions<ServerDbContext> options) : DbContext(options)
{
    public const string ConnectionStringKey = "Pgsql";

    public DbSet<ServerEntity> Servers { get; set; } = null!;
    public DbSet<ServerEntityLogin> LoginServers { get; set; } = null!;
    public DbSet<ServerEntityGame> GameServers { get; set; } = null!;
    public DbSet<ServerEntityShop> ShopServers { get; set; } = null!;
    public DbSet<ServerEntityTrade> TradeServers { get; set; } = null!;

    public DbSet<SessionEntity> Sessions { get; set; } = null!;
    public DbSet<MigrationEntity> Migrations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(ServerDbContext).Assembly);
}
