using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerShopConfiguration : IEntityTypeConfiguration<ServerShopEntity>
{
    public void Configure(EntityTypeBuilder<ServerShopEntity> builder)
    {
        builder.ToTable("servers");
        builder.HasDiscriminator().HasValue("Shop");
    }
}
