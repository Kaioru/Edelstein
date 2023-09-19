using Edelstein.Common.Services.Social.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Social.Configurations;

public class PartyMemberConfiguration : IEntityTypeConfiguration<PartyMemberEntity>
{
    public void Configure(EntityTypeBuilder<PartyMemberEntity> builder)
    {
        builder.ToTable("party_members");

        builder.HasKey(m => m.ID);
        builder.HasKey(m => m.CharacterID);
        builder
            .HasOne(m => m.Party)
            .WithMany(p => p.Members)
            .HasForeignKey(m => m.PartyID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
