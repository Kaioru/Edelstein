namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext>
{
    IPluginHostManifest? Manifest { get; }
    IPluginManager<TContext> Manager { get; }
}
