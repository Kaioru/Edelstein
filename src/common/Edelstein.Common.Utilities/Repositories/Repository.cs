using System.Collections.Concurrent;
using System.Collections.Frozen;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Utilities.Repositories;

public class Repository<TKey, TEntry> : IRepository<TKey, TEntry>
    where TKey : notnull
    where TEntry : IIdentifiable<TKey>
{
    private IDictionary<TKey, TEntry> _dictionary;

    public Repository() => _dictionary = new ConcurrentDictionary<TKey, TEntry>();

    public virtual Task<TEntry?> Retrieve(TKey key)
        => Task.FromResult(_dictionary.TryGetValue(key, out var result) ? result : default);

    public virtual Task<TEntry> Insert(TEntry entry)
        => Task.FromResult(_dictionary[entry.ID] = entry);

    public virtual Task<TEntry> Update(TEntry entry)
        => Task.FromResult(_dictionary[entry.ID] = entry);

    public virtual Task Delete(TKey key)
        => Task.FromResult(_dictionary.Remove(key));

    public virtual Task Delete(TEntry entry)
        => Task.FromResult(_dictionary.Remove(entry.ID));

    public virtual Task<ICollection<TEntry>> RetrieveAll()
        => Task.FromResult(_dictionary.Values);
}
