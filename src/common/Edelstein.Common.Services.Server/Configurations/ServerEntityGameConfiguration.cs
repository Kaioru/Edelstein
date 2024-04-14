using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerEntityGameConfiguration : IEntityTypeConfiguration<ServerEntityGame>
{
    public void Configure(EntityTypeBuilder<ServerEntityGame> builder)
    {
        builder.ToTable("servers");

        builder.HasDiscriminator().HasValue("Game");
    }
}
