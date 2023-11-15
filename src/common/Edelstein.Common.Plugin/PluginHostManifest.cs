using Edelstein.Protocol.Plugin;

namespace Edelstein.Common.Plugin;

public record PluginHostManifest : IPluginHostManifest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string EntryPoint { get; set; }
}
