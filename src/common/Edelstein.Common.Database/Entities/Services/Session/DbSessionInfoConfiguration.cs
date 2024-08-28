using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Session;

public class DbSessionInfoConfiguration : IEntityTypeConfiguration<DbSessionInfo>
{
    public void Configure(EntityTypeBuilder<DbSessionInfo> builder)
    {
        builder.ToTable("session_info");

        builder.HasKey(e => e.ActiveAccount);
        
        builder
            .HasOne(m => m.Account)
            .WithOne(p => p.Session)
            .HasForeignKey<DbSessionInfo>(m => m.ActiveAccount)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(m => m.Character)
            .WithOne(p => p.Session)
            .HasForeignKey<DbSessionInfo>(m => m.ActiveCharacter)
            .OnDelete(DeleteBehavior.SetNull);
        builder
            .HasOne(m => m.Server)
            .WithMany(p => p.Sessions)
            .HasForeignKey(m => m.ServerID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
