using Edelstein.Protocol.Plugin;

namespace Edelstein.Common.Plugin;

public record PluginHost<TContext>(
    IPluginHostManifest? Manifest,
    IPluginManager<TContext> Manager
) : IPluginHost<TContext>;
