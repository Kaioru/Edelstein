using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerTradeConfiguration : IEntityTypeConfiguration<ServerTradeEntity>
{
    public void Configure(EntityTypeBuilder<ServerTradeEntity> builder)
    {
        builder.ToTable("servers");
        builder.HasDiscriminator().HasValue("Trade");
    }
}
