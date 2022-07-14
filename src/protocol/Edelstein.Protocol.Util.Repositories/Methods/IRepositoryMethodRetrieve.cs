namespace Edelstein.Protocol.Util.Repositories.Methods;

public interface IRepositoryMethodRetrieve<in TKey, TEntry> where TEntry : IIdentifiable<TKey>
{
    Task<TEntry> Retrieve(TKey key);
}
