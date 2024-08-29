using Edelstein.Common.Database.Converters;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
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
        
        builder
            .Property(e => e.Inventories)
            .HasJsonConversion(new CharacterInventories());
    }
}
