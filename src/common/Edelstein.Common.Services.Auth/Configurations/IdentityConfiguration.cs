using Edelstein.Common.Services.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Auth.Configurations;

public class IdentityConfiguration : IEntityTypeConfiguration<IdentityEntity>
{
    public void Configure(EntityTypeBuilder<IdentityEntity> builder)
    {
        builder.ToTable("identities");

        builder.HasKey(e => e.ID);
        builder.HasIndex(e => e.Username).IsUnique();
    }
}
