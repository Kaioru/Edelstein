using Edelstein.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<DbAccount> Accounts => Set<DbAccount>();
    
    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
