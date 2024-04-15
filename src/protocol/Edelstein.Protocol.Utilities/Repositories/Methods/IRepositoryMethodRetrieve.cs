using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodRetrieve<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IRepositoryEntry<TKey>
{
    Task<TEntry?> Retrieve(TKey key);
}
