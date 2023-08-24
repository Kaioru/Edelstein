namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodRetrieveAll<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<ICollection<TEntry>> RetrieveAll();
}
