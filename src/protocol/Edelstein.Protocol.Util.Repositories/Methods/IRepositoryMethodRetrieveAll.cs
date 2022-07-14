namespace Edelstein.Protocol.Util.Repositories.Methods;

public interface IRepositoryMethodRetrieveAll<in TKey, TEntry> where TEntry : IIdentifiable<TKey>
{
    Task<IEnumerable<TEntry>> RetrieveAll();
}
