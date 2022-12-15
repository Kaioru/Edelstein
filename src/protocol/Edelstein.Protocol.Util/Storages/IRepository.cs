using Edelstein.Protocol.Util.Storages.Methods;

namespace Edelstein.Protocol.Util.Storages;

public interface IRepository<in TKey, TEntry> :
    IReadOnlyRepository<TKey, TEntry>,
    IRepositoryMethodAsyncInsert<TKey, TEntry>,
    IRepositoryMethodAsyncUpdate<TKey, TEntry>,
    IRepositoryMethodAsyncDelete<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
