namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IStorageMethodRetrieve<in TKey, out TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    TEntry? Retrieve(TKey key);
}
