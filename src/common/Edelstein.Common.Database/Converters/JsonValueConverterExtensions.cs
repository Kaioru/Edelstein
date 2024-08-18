using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edelstein.Common.Database.Converters;

public static class JsonValueConverterExtensions
{
    public static PropertyBuilder HasJsonConversion<TObject>(this PropertyBuilder<TObject> builder, TObject? defaultValue = default)
    {
        return builder
            .HasColumnType("jsonb")
            .HasConversion<JsonValueConverter<TObject>>()
            .HasDefaultValue(defaultValue);
    }
}
