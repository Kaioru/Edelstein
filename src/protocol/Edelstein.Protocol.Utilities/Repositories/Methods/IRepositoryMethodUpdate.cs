namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodUpdate<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    Task<TEntry> Update(TEntry entry);
}
