using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Migration;

public class DbMigrationInfoConfiguration : IEntityTypeConfiguration<DbMigrationInfo>
{
    public void Configure(EntityTypeBuilder<DbMigrationInfo> builder)
    {
        builder.ToTable("migration_info");

        builder.HasKey(e => new { e.AccountID, e.AccountWorldDataID, e.CharacterID });
        builder.HasIndex(e => e.AccountID).IsUnique();
        builder.HasIndex(e => e.AccountWorldDataID).IsUnique();
        builder.HasIndex(e => e.CharacterID).IsUnique();
        
        builder
            .HasOne(e => e.Account)
            .WithOne(p => p.Migration)
            .HasForeignKey<DbMigrationInfo>(e => e.AccountID)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(e => e.AccountWorldData)
            .WithOne(p => p.Migration)
            .HasForeignKey<DbMigrationInfo>(e => e.AccountWorldDataID)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(e => e.Session)
            .WithOne(p => p.Migration)
            .HasForeignKey<DbMigrationInfo>(e => e.AccountID)
            .OnDelete(DeleteBehavior.Cascade);
        
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
