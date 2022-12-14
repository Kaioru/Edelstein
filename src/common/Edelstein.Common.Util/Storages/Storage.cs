using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Common.Util.Storages;

public class Storage<TKey, TEntry> : IStorage<TKey, TEntry>
    where TEntry : IIdentifiable<TKey>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TEntry> _dictionary;

    public Storage() => _dictionary = new Dictionary<TKey, TEntry>();

    public TEntry? Retrieve(TKey key) => _dictionary.TryGetValue(key, out var entry) ? entry : default;
    public IEnumerable<TEntry> RetrieveAll() => _dictionary.Values;
    public void Insert(TEntry entry) => _dictionary[entry.ID] = entry;
    public void Delete(TKey key) => _dictionary.Remove(key);
    public void Delete(TEntry entry) => _dictionary.Remove(entry.ID);
}
