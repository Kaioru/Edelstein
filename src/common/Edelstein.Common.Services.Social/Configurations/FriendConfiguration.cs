using Edelstein.Common.Services.Social.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Social.Configurations;

public class FriendConfiguration : IEntityTypeConfiguration<FriendEntity>
{
    public void Configure(EntityTypeBuilder<FriendEntity> builder)
    {
        builder.ToTable("friends");

        builder.HasKey(m => m.ID);
        builder.HasIndex(m => new
        {
            m.CharacterID,
            m.FriendID
        }).IsUnique();
        builder
            .HasOne(m => m.Profile)
            .WithMany(p => p.Friends)
            .HasForeignKey(m => m.CharacterID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
