using Edelstein.Common.Database.Entities;
using Edelstein.Common.Database.Entities.Services.Auth;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Common.Database.Entities.Services.Session;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<DbIdentity> Identities => Set<DbIdentity>();
    
    public DbSet<DbServerInfo> ServerInfo => Set<DbServerInfo>();
    public DbSet<DbServerInfoLogin> ServerInfoLogin => Set<DbServerInfoLogin>();
    public DbSet<DbServerInfoGame> ServerInfoGame => Set<DbServerInfoGame>();
    
    public DbSet<DbSessionInfo> SessionInfo => Set<DbSessionInfo>();
    public DbSet<DbMigrationInfo> MigrationInfo => Set<DbMigrationInfo>();
    
    public DbSet<DbAccount> Accounts => Set<DbAccount>();
    public DbSet<DbAccountWorldData> AccountWorldData => Set<DbAccountWorldData>();
    public DbSet<DbCharacter> Characters => Set<DbCharacter>();
    
    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
