using System.Collections.Concurrent;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Utilities.Repositories;

public class Repository<TKey, TEntry> : IRepository<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    private readonly IDictionary<TKey, TEntry> _dictionary;

    public Repository() => _dictionary = new ConcurrentDictionary<TKey, TEntry>();

    public Task<TEntry?> Retrieve(TKey key)
        => Task.FromResult(_dictionary.TryGetValue(key, out var result) ? result : default);

    public Task<TEntry> Insert(TEntry entry)
        => Task.FromResult(_dictionary[entry.ID] = entry);

    public Task<TEntry> Update(TEntry entry)
        => Task.FromResult(_dictionary[entry.ID] = entry);

    public Task Delete(TKey key)
        => Task.FromResult(_dictionary.Remove(key));

    public Task Delete(TEntry entry)
        => Task.FromResult(_dictionary.Remove(entry.ID));

    public Task<ICollection<TEntry>> RetrieveAll()
        => Task.FromResult(_dictionary.Values);
}
