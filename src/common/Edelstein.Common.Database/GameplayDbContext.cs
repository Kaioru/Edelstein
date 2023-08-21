using Edelstein.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database;

public class GameplayDbContext : DbContext
{
    public const string ConnectionStringKey = "Gameplay";

    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AccountWorldEntity> AccountWorlds { get; set; }
    public DbSet<CharacterEntity> Characters { get; set; }
    
    public GameplayDbContext(DbContextOptions<GameplayDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(GameplayDbContext).Assembly);
}

