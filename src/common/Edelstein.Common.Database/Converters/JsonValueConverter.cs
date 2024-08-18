using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Edelstein.Common.Database.Converters;

public class JsonValueConverter<T>() : ValueConverter<T, string>(
    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
    s => JsonSerializer.Deserialize<T>(s, (JsonSerializerOptions?)null)!
);
