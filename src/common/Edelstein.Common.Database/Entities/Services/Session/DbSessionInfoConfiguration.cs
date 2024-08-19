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
            .HasOne(m => m.Server)
            .WithMany(p => p.Sessions)
            .HasForeignKey(m => m.ServerID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
