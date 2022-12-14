namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IStorageMethodDelete<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    void Delete(TKey key);
    void Delete(TEntry entry);
}
