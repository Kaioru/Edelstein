using Edelstein.Common.Services.Social.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Social.Configurations;

public class PartyInvitationConfiguration : IEntityTypeConfiguration<PartyInvitationEntity>
{
    public void Configure(EntityTypeBuilder<PartyInvitationEntity> builder)
    {
        builder.ToTable("party_invitations");

        builder.HasKey(m => m.ID);
        builder.HasIndex(m => new
        {
            m.PartyID,
            m.CharacterID
        }).IsUnique();
        builder
            .HasOne(m => m.Party)
            .WithMany(p => p.Invitations)
            .HasForeignKey(m => m.PartyID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
