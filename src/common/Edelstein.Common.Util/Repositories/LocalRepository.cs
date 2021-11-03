using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Util.Repositories
{
    public class LocalRepository<TKey, TEntry> : ILocalRepository<TKey, TEntry> where TEntry : class, IRepositoryEntry<TKey>
    {
        protected IDictionary<TKey, TEntry> Dictionary;

        public LocalRepository(IDictionary<TKey, TEntry> dictionary = null)
        {
            Dictionary = dictionary ?? new Dictionary<TKey, TEntry>();
        }

        public Task<bool> Exists(TKey key)
            => Task.FromResult(Dictionary.ContainsKey(key));

        public virtual Task<TEntry> Retrieve(TKey key)
        {
            if (Dictionary.ContainsKey(key))
                return Task.FromResult(Dictionary[key]);
            return Task.FromResult<TEntry>(default);
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<TKey> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));


        public virtual Task Insert(TEntry entry)
        {
            Dictionary[entry.ID] = entry;
            return Task.CompletedTask;
        }

        public Task Insert(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Insert(entry)));

        public virtual Task Update(TEntry entry)
        {
            Dictionary[entry.ID] = entry;
            return Task.CompletedTask;
        }

        public Task Update(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Update(entry)));

        public virtual Task Delete(TKey key)
        {
            Dictionary.Remove(key);
            return Task.CompletedTask;
        }

        public Task Delete(TEntry entry)
            => Delete(entry.ID);

        public Task Delete(IEnumerable<TKey> keys)
            => Task.WhenAll(keys.Select(key => Delete(key)));

        public Task Delete(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Delete(entry)));

        public async Task<IEnumerable<TEntry>> RetrieveAll() => await Task.WhenAll(Dictionary.Keys.Select(k => Retrieve(k)));
        public Task DeleteAll() => Delete(Dictionary.Values);
    }
}
