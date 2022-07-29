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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ServerModel>().HasDiscriminator().HasValue("Server");
        builder.Entity<ServerLoginModel>().HasDiscriminator().HasValue("Login");
        builder.Entity<ServerGameModel>().HasDiscriminator().HasValue("Game");
    }
}
