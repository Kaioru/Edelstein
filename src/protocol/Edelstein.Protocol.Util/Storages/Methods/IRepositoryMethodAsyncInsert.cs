namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IRepositoryMethodAsyncInsert<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<TEntry> Insert(TEntry entry);
}
