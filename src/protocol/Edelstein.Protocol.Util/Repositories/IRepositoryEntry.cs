namespace Edelstein.Protocol.Util.Repositories
{
    public interface IRepositoryEntry<TKey>
    {
        TKey ID { get; }
    }
}
