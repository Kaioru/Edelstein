using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Repositories
{
    public interface ILocalRepositoryReader<
        TKey,
        TEntry
    > : IRepositoryReader<TKey, TEntry>
        where TEntry : class, IRepositoryEntry<TKey>
    {
        Task<IEnumerable<TEntry>> RetrieveAll();
    }
}
