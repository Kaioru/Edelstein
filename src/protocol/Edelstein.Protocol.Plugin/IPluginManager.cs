using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Plugin;

public interface IPluginManager<TContext> : 
    IRepositoryMethodRetrieve<string, IPlugin<TContext>>,
    IRepositoryMethodRetrieveAll<string, IPlugin<TContext>>,
    IRepositoryMethodInsert<string, IPlugin<TContext>>
{
    Task LoadFromFile(string path);
    Task LoadFromDirectory(string directory);
}
