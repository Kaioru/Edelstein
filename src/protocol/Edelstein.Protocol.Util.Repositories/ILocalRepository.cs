using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Util.Repositories;

public interface ILocalRepository<in TKey, TEntry> :
    IRepository<TKey, TEntry>,
    IRepositoryMethodRetrieveAll<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
