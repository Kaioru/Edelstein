using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Plugin;

public interface IPluginManager<TContext> : 
    IRepositoryMethodRetrieve<string, IPluginHost<TContext>>,
    IRepositoryMethodRetrieveAll<string, IPluginHost<TContext>>,
    IRepositoryMethodInsert<string, IPluginHost<TContext>>
{
    Task LoadFromFile(string path);
    Task LoadFromDirectory(string directory);
}
