using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Server;

public class DbServerInfoGameConfiguration : IEntityTypeConfiguration<DbServerInfoGame>
{
    public void Configure(EntityTypeBuilder<DbServerInfoGame> builder)
    {
        builder.ToTable("server_info");

        builder.HasDiscriminator().HasValue("Game");
    }
}
