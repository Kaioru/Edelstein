using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Repositories
{
    public interface ILocalRepositoryWriter<
        TKey,
        TEntry
    > : IRepositoryWriter<TKey, TEntry>
        where TEntry : class, IRepositoryEntry<TKey>
    {
        Task DeleteAll();
    }
}
