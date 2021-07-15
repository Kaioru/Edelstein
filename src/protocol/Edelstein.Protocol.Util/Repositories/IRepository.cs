namespace Edelstein.Protocol.Util.Repositories
{
    public interface IRepository<
        TKey,
        TEntry
    > : IRepositoryReader<TKey, TEntry>,
        IRepositoryWriter<TKey, TEntry>
        where TEntry : class, IRepositoryEntry<TKey>
    {
    }
}