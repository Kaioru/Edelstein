using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Caching;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Util.Repositories
{
    public class CachedRepository<TKey, TEntry> : IRepository<TKey, TEntry> where TEntry : class, IRepositoryEntry<TKey>
    {
        protected ICache Cache { get; }
        protected IRepository<TKey, TEntry> Repository { get; }
        protected TimeSpan Duration { get; }

        public CachedRepository(ICache cache, IRepository<TKey, TEntry> repository, TimeSpan duration)
        {
            Cache = cache;
            Repository = repository;
            Duration = duration;
        }

        public async Task<TEntry> Retrieve(TKey key)
        {
            var cached = await Cache.Get<TEntry>(key.ToString());
            if (cached != null)
            {
                _ = Cache.Refresh(key.ToString());
                return cached;
            }
            var result = await Repository.Retrieve(key);
            if (result != null)
                await Cache.Set(result.ID.ToString(), result, Duration);
            return result;
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<TKey> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));


        public async Task Insert(TEntry entry)
        {
            await Cache.Set(entry.ID.ToString(), entry, Duration);
            await Repository.Insert(entry);
        }

        public Task Insert(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Insert(entry)));

        public async Task Update(TEntry entry)
        {
            await Cache.Set(entry.ID.ToString(), entry, Duration);
            await Repository.Update(entry);
        }

        public Task Update(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Update(entry)));

        public async Task Delete(TKey key)
        {
            await Cache.Remove(key.ToString());
            await Repository.Delete(key);
        }

        public async Task Delete(TEntry entry)
        {
            await Cache.Remove(entry.ID.ToString());
            await Repository.Delete(entry);
        }

        public Task Delete(IEnumerable<TKey> keys)
            => Task.WhenAll(keys.Select(key => Delete(key)));

        public Task Delete(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Delete(entry)));
    }
}
