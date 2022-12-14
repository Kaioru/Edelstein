using Edelstein.Protocol.Util.Storages.Methods;

namespace Edelstein.Protocol.Util.Storages;

public interface IStorage<in TKey, TEntry> :
    IStorageMethodRetrieve<TKey, TEntry>,
    IStorageMethodRetrieveAll<TKey, TEntry>,
    IStorageMethodInsert<TKey, TEntry>,
    IStorageMethodDelete<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
