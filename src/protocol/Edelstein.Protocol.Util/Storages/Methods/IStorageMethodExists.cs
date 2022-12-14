namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IStorageMethodExists<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    bool Exists(TKey key);
    bool Exists(TEntry entry);
}
