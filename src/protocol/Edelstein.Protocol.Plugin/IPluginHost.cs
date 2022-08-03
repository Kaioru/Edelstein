using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext>
{
    ILogger Logger { get; }
    IPluginCollection<TContext> Plugins { get; }
}
