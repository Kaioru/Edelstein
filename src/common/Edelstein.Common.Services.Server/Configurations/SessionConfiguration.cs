using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(m => m.ActiveAccount);
        builder
            .HasOne(m => m.Server)
            .WithMany(p => p.Sessions)
            .HasForeignKey(m => m.ServerID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
