using Edelstein.Common.Services.Session.Models;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Session;

public class SessionDbContext : DbContext
{
    public const string ConnectionStringKey = "Session";

    public SessionDbContext(DbContextOptions<SessionDbContext> options) : base(options)
    {
    }

    public DbSet<SessionModel> Sessions { get; set; }
}
