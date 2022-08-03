namespace Edelstein.Protocol.Plugin;

public interface IPluginManager<in TContext>
{
    Task Load(string path);
    Task LoadFrom(string directory);

    Task Start();
    Task Stop();
}
