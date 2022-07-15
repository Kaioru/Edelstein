using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Util.Repositories;

public class InMemoryRepository<TKey, TEntry> : ILocalRepository<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    private readonly Dictionary<TKey, TEntry> _dictionary;

    public InMemoryRepository()
        => _dictionary = new Dictionary<TKey, TEntry>();

    public Task<TEntry?> Retrieve(TKey key)
        => Task.FromResult(_dictionary.GetValueOrDefault(key));

    public Task<IEnumerable<TEntry>> RetrieveAll()
        => Task.FromResult<IEnumerable<TEntry>>(_dictionary.Values);

    public Task<TEntry> Insert(TEntry entry)
    {
        _dictionary[entry.ID] = entry;
        return Task.FromResult(entry);
    }

    public Task<TEntry> Update(TEntry entry)
    {
        _dictionary[entry.ID] = entry;
        return Task.FromResult(entry);
    }

    public Task Delete(TKey key)
        => Task.FromResult(_dictionary.Remove(key));

    public Task Delete(TEntry entry)
        => Task.FromResult(_dictionary.Remove(entry.ID));
}
