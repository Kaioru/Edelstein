namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IStorageMethodRetrieveAll<in TKey, out TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    IEnumerable<TEntry> RetrieveAll();
}
