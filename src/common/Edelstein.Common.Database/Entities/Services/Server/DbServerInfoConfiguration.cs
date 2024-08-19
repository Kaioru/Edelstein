using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Server;

public class DbServerInfoConfiguration : IEntityTypeConfiguration<DbServerInfo>
{
    public void Configure(EntityTypeBuilder<DbServerInfo> builder)
    {
        builder.ToTable("server_info");

        builder.HasKey(e => e.ID);
        builder.HasDiscriminator().HasValue("Server");
    }
}
