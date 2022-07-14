namespace Edelstein.Protocol.Util.Repositories.Methods;

public interface IRepositoryMethodDelete<in TKey, in TEntry> where TEntry : IIdentifiable<TKey>
{
    Task Delete(TKey key);
    Task Delete(TEntry entry);
}
