namespace Edelstein.Protocol.Util.Repositories
{
    public interface ILocalRepository<
        TKey,
        TEntry
    > : ILocalRepositoryReader<TKey, TEntry>,
        ILocalRepositoryWriter<TKey, TEntry>
        where TEntry : class, IRepositoryEntry<TKey>
    {
    }
}