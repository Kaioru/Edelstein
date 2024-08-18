using Edelstein.Common.Database.Entities;
using Edelstein.Common.Database.Entities.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<DbIdentity> Identities => Set<DbIdentity>();
    
    public DbSet<DbAccount> Accounts => Set<DbAccount>();
    public DbSet<DbAccountWorldData> AccountWorldData => Set<DbAccountWorldData>();
    
    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
