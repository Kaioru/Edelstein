using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Repositories
{
    public interface IRepositoryWriter<
        TKey,
        TEntry
    > where TEntry : class, IRepositoryEntry<TKey>
    {
        Task Insert(TEntry entry);
        Task Insert(IEnumerable<TEntry> entries);

        Task Update(TEntry entry);
        Task Update(IEnumerable<TEntry> entries);

        Task Delete(TKey key);
        Task Delete(TEntry entry);
        Task Delete(IEnumerable<TKey> keys);
        Task Delete(IEnumerable<TEntry> entries);
    }
}
