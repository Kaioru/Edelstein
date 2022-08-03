using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Plugin;

public interface IPluginCollection<TContext> : IRepositoryMethodRetrieve<string, IPlugin<TContext>>
{
}
