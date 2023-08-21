namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodRetrieve<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<TEntry?> Retrieve(TKey key);
}
