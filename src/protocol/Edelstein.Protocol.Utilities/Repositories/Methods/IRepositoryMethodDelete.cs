using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodDelete<in TKey, in TEntry>
    where TKey : notnull
    where TEntry : IRepositoryEntry<TKey>
{
    Task Delete(TKey key);
    Task Delete(TEntry entry);
}
