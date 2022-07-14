using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Util.Repositories;

public interface IRepository<in TKey, TEntry> :
    IRepositoryMethodRetrieve<TKey, TEntry>,
    IRepositoryMethodInsert<TKey, TEntry>,
    IRepositoryMethodUpdate<TKey, TEntry>,
    IRepositoryMethodDelete<TKey, TEntry>
    where TEntry : IIdentifiable<TKey>
{
}
