namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IStorageMethodInsert<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    void Insert(TEntry entry);
}
