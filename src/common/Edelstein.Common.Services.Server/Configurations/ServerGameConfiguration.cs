using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerGameConfiguration: IEntityTypeConfiguration<ServerGameEntity>
{
    public void Configure(EntityTypeBuilder<ServerGameEntity> builder)
    {
        builder.ToTable("servers");
        builder.HasDiscriminator().HasValue("Game");
    }
}
