using Edelstein.Common.Services.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Auth;

public class AuthDbContext : DbContext
{
    public const string ConnectionStringKey = "Auth";

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<IdentityEntity> Identities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
}
