using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class MigrationEntityConfiguration : IEntityTypeConfiguration<MigrationEntity>
{
    public void Configure(EntityTypeBuilder<MigrationEntity> builder)
    {
        builder.ToTable("migrations");

        builder.HasKey(m => m.AccountID);
        builder.HasKey(m => m.CharacterID);
        builder
            .HasOne(m => m.FromServer)
            .WithMany(p => p.MigrationOut)
            .HasForeignKey(m => m.FromServerID)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(m => m.ToServer)
            .WithMany(p => p.MigrationIn)
            .HasForeignKey(m => m.ToServerID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
