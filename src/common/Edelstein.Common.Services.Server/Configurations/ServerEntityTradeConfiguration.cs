using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerEntityTradeConfiguration : IEntityTypeConfiguration<ServerEntityTrade>
{
    public void Configure(EntityTypeBuilder<ServerEntityTrade> builder)
    {
        builder.ToTable("servers");

        builder.HasDiscriminator().HasValue("Trade");
    }
}
