namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodDelete<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task Delete(TKey key);
    Task Delete(TEntry entry);
}
