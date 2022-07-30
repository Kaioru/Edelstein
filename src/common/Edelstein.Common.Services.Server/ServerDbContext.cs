using Edelstein.Common.Services.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class ServerDbContext : DbContext
{
    public const string ConnectionStringKey = "Server";

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    public DbSet<ServerModel> Servers { get; set; }
    public DbSet<ServerLoginModel> LoginServers { get; set; }
    public DbSet<ServerGameModel> GameServers { get; set; }

    public DbSet<SessionModel> Sessions { get; set; }
    public DbSet<MigrationModel> Migrations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ServerModel>().HasDiscriminator().HasValue("Server");
        builder.Entity<ServerLoginModel>().HasDiscriminator().HasValue("Login");
        builder.Entity<ServerGameModel>().HasDiscriminator().HasValue("Game");

        builder.Entity<SessionModel>().HasKey(m => m.ActiveAccount);
        builder.Entity<SessionModel>()
            .HasOne(m => m.Server)
            .WithMany(p => p.Sessions)
            .HasForeignKey(m => m.ServerID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MigrationModel>()
            .HasOne(m => m.FromServer)
            .WithMany(p => p.MigrationOut)
            .HasForeignKey(m => m.FromServerID)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<MigrationModel>()
            .HasOne(m => m.ToServer)
            .WithMany(p => p.MigrationIn)
            .HasForeignKey(m => m.ToServerID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
