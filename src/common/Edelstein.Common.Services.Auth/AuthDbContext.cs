using Edelstein.Common.Services.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Auth;

public class AuthDbContext : DbContext
{
    public const string ConnectionStringKey = "Auth";

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<IdentityModel> Identities { get; set; }
}
