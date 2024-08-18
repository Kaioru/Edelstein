using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Entities;

public class DbAccountConfiguration : IEntityTypeConfiguration<DbAccount>
{
    public void Configure(EntityTypeBuilder<DbAccount> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(e => e.ID);
        builder.HasIndex(e => e.Username).IsUnique();
    }
}
