using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Repositories.Methods;

public interface IRepositoryMethodUpdate<in TKey, TEntry>
    where TKey : notnull
    where TEntry : IRepositoryEntry<TKey>
{
    Task<TEntry> Update(TEntry entry);
}
