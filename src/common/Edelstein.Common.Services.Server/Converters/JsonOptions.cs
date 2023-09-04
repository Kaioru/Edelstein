using Newtonsoft.Json;

namespace Edelstein.Common.Services.Server.Converters;

internal static class JsonOptions
{
    internal static readonly JsonSerializerSettings Settings = new()
    {
        Formatting = Formatting.None,
        TypeNameHandling = TypeNameHandling.All
    };
}
