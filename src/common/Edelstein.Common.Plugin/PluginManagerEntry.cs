using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Plugin;

public record PluginManagerEntry<TContext>(
    IPluginHost<TContext> Host,
    IPlugin<TContext> Plugin
) : IRepositoryEntry<string>
{
    public string ID => Plugin.ID;
}
