using Edelstein.Common.Services.Server.Converters;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class MigrationConfiguration : IEntityTypeConfiguration<MigrationEntity>
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

        builder
            .Property(e => e.Account)
            .HasColumnType("json")
            .HasConversion(new JsonConverter<IAccount>());
        builder
            .Property(e => e.AccountWorld)
            .HasColumnType("json")
            .HasConversion(new JsonConverter<IAccountWorld>());
        builder
            .Property(e => e.Character)
            .HasColumnType("json")
            .HasConversion(new JsonConverter<ICharacter>());
    }
}
