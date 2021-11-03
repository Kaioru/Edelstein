using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Repositories
{
    public interface IRepositoryReader<
        TKey,
        TEntry
    > where TEntry : class, IRepositoryEntry<TKey>
    {
        Task<bool> Exists(TKey key);

        Task<TEntry> Retrieve(TKey key);
        Task<IEnumerable<TEntry>> Retrieve(IEnumerable<TKey> keys);
    }
}
