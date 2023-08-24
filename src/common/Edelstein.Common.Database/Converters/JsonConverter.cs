using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Edelstein.Common.Database.Converters;

public class JsonConverter<T> : ValueConverter<T, string>
{
    public JsonConverter() : base(
        v => JsonConvert.SerializeObject(v, JsonOptions.Settings),
        s => (T)JsonConvert.DeserializeObject<T>(s, JsonOptions.Settings)!
    )
    {
    }
}
