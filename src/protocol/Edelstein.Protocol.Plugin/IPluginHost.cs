using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost
{
    ILogger Logger { get; }
}
