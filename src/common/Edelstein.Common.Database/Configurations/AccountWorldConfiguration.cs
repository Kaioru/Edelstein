using Edelstein.Common.Database.Converters;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Configurations;

public class AccountWorldConfiguration : IEntityTypeConfiguration<AccountWorldEntity>
{
    public void Configure(EntityTypeBuilder<AccountWorldEntity> builder)
    {
        builder.ToTable("account_worlds");

        builder.HasKey(e => e.ID);
        builder.HasOne(e => e.Account)
            .WithMany(p => p.AccountWorlds)
            .HasForeignKey(e => e.AccountID)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.Locker)
            .HasColumnType("json")
            .HasConversion<JsonConverter<IItemLocker>>()
            .HasDefaultValue(new ItemLocker());
        builder
            .Property(e => e.Trunk)
            .HasColumnType("json")
            .HasConversion<JsonConverter<IItemTrunk>>()
            .HasDefaultValue(new ItemTrunk());
    }
}
