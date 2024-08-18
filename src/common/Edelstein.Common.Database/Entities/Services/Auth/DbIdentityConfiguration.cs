using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Auth;

public class DbIdentityConfiguration : IEntityTypeConfiguration<DbIdentity>
{
    public void Configure(EntityTypeBuilder<DbIdentity> builder)
    {
        builder.ToTable("identities");
        
        builder.HasKey(e => e.ID);
        builder.HasIndex(e => e.Username).IsUnique();
    }
}
