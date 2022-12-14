namespace Edelstein.Protocol.Util.Storages.Methods;

public interface IRepositoryMethodAsyncRetrieveAll<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<IEnumerable<TEntry>> RetrieveAll();
}
