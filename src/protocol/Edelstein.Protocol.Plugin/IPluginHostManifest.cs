namespace Edelstein.Protocol.Plugin;

public interface IPluginHostManifest
{
    string Name { get; }
    string Description { get; }
    
    string EntryPoint { get; }
}
