using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Utilities.Repositories;

public interface IReadOnlyRepository<in TKey, TEntry> :
    IRepositoryMethodRetrieve<TKey, TEntry>,
    IRepositoryMethodRetrieveAll<TKey, TEntry> 
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
}
