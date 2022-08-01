using Edelstein.Common.Gameplay.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Gameplay.Database;

public class GameplayDbContext : DbContext
{
    public const string ConnectionStringKey = "Gameplay";

    public GameplayDbContext(DbContextOptions<GameplayDbContext> options) : base(options)
    {
    }

    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<AccountWorldModel> AccountWorlds { get; set; }
    public DbSet<CharacterModel> Characters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AccountModel>(entity => entity.HasIndex(e => e.Username).IsUnique());

        builder.Entity<AccountWorldModel>()
            .HasOne(m => m.Account)
            .WithMany(p => p.AccountWorlds)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CharacterModel>(entity => entity.HasIndex(e => e.Name).IsUnique());
        builder.Entity<CharacterModel>()
            .HasOne(m => m.AccountWorld)
            .WithMany(p => p.Characters)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
