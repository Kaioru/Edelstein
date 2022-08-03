using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public record PluginHost<TContext>(
    ILogger Logger,
    IPluginCollection<TContext> Plugins
) : IPluginHost<TContext>;
