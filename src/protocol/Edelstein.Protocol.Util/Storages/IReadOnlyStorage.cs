using Edelstein.Protocol.Util.Storages.Methods;

namespace Edelstein.Protocol.Util.Storages;

public interface IReadOnlyStorage<in TKey, out TEntry> :
    IStorageMethodRetrieve<TKey, TEntry>,
    IStorageMethodRetrieveAll<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
