using Edelstein.Common.Services.Social.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Social.Configurations;

public class FriendProfileConfiguration : IEntityTypeConfiguration<FriendProfileEntity>
{
    public void Configure(EntityTypeBuilder<FriendProfileEntity> builder)
    {
        builder.ToTable("friend_profiles");

        builder.HasKey(m => m.CharacterID);
    }
}
