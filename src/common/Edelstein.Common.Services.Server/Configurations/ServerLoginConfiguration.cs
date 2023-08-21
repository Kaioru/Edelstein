using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerLoginConfiguration: IEntityTypeConfiguration<ServerLoginEntity>
{
    public void Configure(EntityTypeBuilder<ServerLoginEntity> builder)
    {
        builder.ToTable("servers");
        builder.HasDiscriminator().HasValue("Login");
    }
}
