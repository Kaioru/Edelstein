namespace Edelstein.Protocol.Util.Repositories.Methods;

public interface IRepositoryMethodRetrieveAll<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<IEnumerable<TEntry>> RetrieveAll();
}
