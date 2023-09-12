using Edelstein.Common.Database.Converters;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters;
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
            .Property(e => e.ExtendSP)
            .HasColumnType("json")
            .HasConversion<JsonConverter<ICharacterExtendSP>>()
            .HasDefaultValue(new CharacterExtendSP());
        
        builder
            .Property(e => e.Inventories)
            .HasColumnType("json")
            .HasConversion<JsonConverter<ICharacterInventories>>()
            .HasDefaultValue(new CharacterInventories());
        builder
            .Property(e => e.Skills)
            .HasColumnType("json")
            .HasConversion<JsonConverter<ICharacterSkills>>()
            .HasDefaultValue(new CharacterSkills());
        builder
            .Property(e => e.WildHunterInfo)
            .HasColumnType("json")
            .HasConversion<JsonConverter<ICharacterWildHunterInfo>>()
            .HasDefaultValue(new CharacterWildHunterInfo());

        builder
            .Property(e => e.TemporaryStats)
            .HasColumnType("json")
            .HasConversion<JsonConverter<ICharacterTemporaryStats>>()
            .HasDefaultValue(new CharacterTemporaryStats());
    }
}
