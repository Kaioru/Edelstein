using Edelstein.Common.Database.Converters;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<CharacterEntity>
{
    public void Configure(EntityTypeBuilder<CharacterEntity> builder)
    {
        builder.ToTable("characters");
        
        builder.HasKey(e => e.ID);
        builder.HasOne(e => e.AccountWorld)
            .WithMany(p => p.Characters)
            .HasForeignKey(e => e.AccountWorldID)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.Inventories)
            .HasColumnType("jsonb")
            .HasConversion<JsonConverter<IDictionary<ItemInventoryType, IItemInventory>>>();
    }
}
