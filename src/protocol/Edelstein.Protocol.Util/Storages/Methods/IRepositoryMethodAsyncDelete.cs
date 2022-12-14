namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IRepositoryMethodAsyncDelete<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task Delete(TKey key);
    Task Delete(TEntry entry);
}
