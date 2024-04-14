using Edelstein.Common.Services.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Services.Server.Configurations;

public class ServerEntityLoginConfiguration : IEntityTypeConfiguration<ServerEntityLogin>
{
    public void Configure(EntityTypeBuilder<ServerEntityLogin> builder)
    {
        builder.ToTable("servers");

        builder.HasDiscriminator().HasValue("Login");
    }
}
