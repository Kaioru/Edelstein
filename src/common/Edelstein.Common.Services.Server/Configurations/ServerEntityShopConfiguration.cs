using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerEntityShopConfiguration : IEntityTypeConfiguration<ServerEntityShop>
{
    public void Configure(EntityTypeBuilder<ServerEntityShop> builder)
    {
        builder.ToTable("servers");

        builder.HasDiscriminator().HasValue("Shop");
    }
}
