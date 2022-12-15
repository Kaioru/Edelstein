using Edelstein.Protocol.Util.Storages.Methods;

namespace Edelstein.Protocol.Util.Storages;

public interface IReadOnlyRepository<in TKey, TEntry> :
    IRepositoryMethodAsyncRetrieve<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
