using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities;

public class DbCharacterConfiguration : IEntityTypeConfiguration<DbCharacter>
{
    public void Configure(EntityTypeBuilder<DbCharacter> builder)
    {
        builder.ToTable("characters");

        builder.HasKey(e => e.ID);
        builder.HasIndex(e => e.Name).IsUnique();
        
        builder.HasOne(e => e.AccountWorldData)
            .WithMany(p => p.Characters)
            .HasForeignKey(e => e.AccountWorldDataID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
