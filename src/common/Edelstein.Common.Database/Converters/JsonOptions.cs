using Newtonsoft.Json;

namespace Edelstein.Common.Database.Converters;

internal static class JsonOptions
{
    internal static JsonSerializerSettings Settings = new()
    {
        Formatting = Formatting.None,
        TypeNameHandling = TypeNameHandling.All
    };
}
