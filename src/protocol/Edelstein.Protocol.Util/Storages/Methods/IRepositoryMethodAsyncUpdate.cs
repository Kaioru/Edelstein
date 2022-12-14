namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IRepositoryMethodAsyncUpdate<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<TEntry> Update(TEntry entry);
}
