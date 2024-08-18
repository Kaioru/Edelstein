using Edelstein.Common.Database.Converters;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities;

public class DbAccountWorldDataConfiguration : IEntityTypeConfiguration<DbAccountWorldData>
{
    public void Configure(EntityTypeBuilder<DbAccountWorldData> builder)
    {
        builder.ToTable("account_world_data");

        builder.HasKey(e => e.ID);
        builder.HasOne(e => e.Account)
            .WithMany(p => p.AccountWorldData)
            .HasForeignKey(e => e.AccountID)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.Locker)
            .HasJsonConversion(new ItemLocker());
        builder
            .Property(e => e.Trunk)
            .HasJsonConversion(new ItemTrunk());
    }
}
