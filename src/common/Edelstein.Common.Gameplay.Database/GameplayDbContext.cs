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

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.Entity<AccountModel>(entity => { entity.HasIndex(e => e.Username).IsUnique(); });
}
