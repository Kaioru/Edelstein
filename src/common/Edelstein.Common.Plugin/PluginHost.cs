using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public record PluginHost<TContext>(
    ILogger Logger,
    IConfiguration Config,
    string DirectoryHost,
    string DirectoryPlugin,
    IPlugin<TContext> Plugin
) : IPluginHost<TContext>
{
    public string ID => Plugin.ID;
}
