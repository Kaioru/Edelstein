using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities.Services.Server;

public class DbServerInfoLoginConfiguration : IEntityTypeConfiguration<DbServerInfoLogin>
{
    public void Configure(EntityTypeBuilder<DbServerInfoLogin> builder)
    {
        builder.ToTable("server_info");

        builder.HasDiscriminator().HasValue("Login");
    }
}
