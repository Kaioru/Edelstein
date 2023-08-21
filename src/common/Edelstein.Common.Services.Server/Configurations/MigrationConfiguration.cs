using Ceras;
using Ceras.Exceptions;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Services.Server.Converters;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class MigrationConfiguration: IEntityTypeConfiguration<MigrationEntity>
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

        var serializer = new CerasSerializer();
        
        builder
            .Property(e => e.Account)
            .HasConversion(new BinaryConverter<IAccount>(serializer));
        builder
            .Property(e => e.AccountWorld)
            .HasConversion(new BinaryConverter<IAccountWorld>(serializer));
        builder
            .Property(e => e.Character)
            .HasConversion(new BinaryConverter<ICharacter>(serializer));
    }
}
