namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IRepositoryMethodAsyncRetrieve<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<TEntry?> Retrieve(TKey key);
}
