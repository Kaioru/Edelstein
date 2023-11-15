using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public record PluginHost<TContext>(
    IPluginHostManifest? Manifest,
    ILogger Logger,
    IConfiguration Config,
    string DirectoryHost,
    string DirectoryPlugin,
    IPluginManager<TContext> Manager,
    IPlugin<TContext> Plugin
) : IPluginHost<TContext>
{
    public string ID => Plugin.ID;
}
