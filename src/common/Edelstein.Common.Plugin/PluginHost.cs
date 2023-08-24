using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public record PluginHost(
    ILogger Logger
) : IPluginHost;
