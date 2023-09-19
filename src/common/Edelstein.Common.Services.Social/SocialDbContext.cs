using Edelstein.Common.Services.Social.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class SocialDbContext : DbContext
{
    public const string ConnectionStringKey = "Social";

    public SocialDbContext(DbContextOptions<SocialDbContext> options) : base(options)
    {
    }

    public DbSet<FriendEntity> Friends { get; set; }
    public DbSet<PartyEntity> Parties { get; set; }
    public DbSet<PartyMemberEntity> PartyMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(SocialDbContext).Assembly);
}
