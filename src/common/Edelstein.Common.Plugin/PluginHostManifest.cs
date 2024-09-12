using System.Collections.Generic;
using Edelstein.Protocol.Plugin;

namespace Edelstein.Common.Plugin;

public record PluginHostManifest : IPluginHostManifest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    
    public required string EntryPoint { get; init; }

    public ICollection<string> Dependencies { get; init; } = new List<string>();
}
