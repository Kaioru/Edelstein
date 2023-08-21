using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Repositories;

public interface IQueriedRepository<in TKey, TEntry> :
    IRepositoryMethodRetrieve<TKey, TEntry>,
    IRepositoryMethodInsert<TKey, TEntry>,
    IRepositoryMethodUpdate<TKey, TEntry>,
    IRepositoryMethodDelete<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
