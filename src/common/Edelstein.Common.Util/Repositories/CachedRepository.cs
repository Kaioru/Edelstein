using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;
using Foundatio.Caching;

namespace Edelstein.Common.Util.Repositories
{
    public class CachedRepository<TKey, TEntry> : IRepository<TKey, TEntry> where TEntry : class, IRepositoryEntry<TKey>
    {
        protected ICacheClient Cache { get; }
        protected IRepository<TKey, TEntry> Repository { get; }
        protected TimeSpan Duration { get; }

        public CachedRepository(ICacheClient cache, IRepository<TKey, TEntry> repository, TimeSpan duration)
        {
            Cache = cache;
            Repository = repository;
            Duration = duration;
        }

        public async Task<TEntry> Retrieve(TKey key)
        {
            var cached = await Cache.GetAsync<TEntry>(key.ToString());
            if (cached.HasValue)
            {
                _ = Cache.SetExpirationAsync(key.ToString(), Duration);
                return cached.Value;
            }
            var result = await Repository.Retrieve(key);
            if (result != null)
                await Cache.SetAsync(result.ID.ToString(), result, Duration);
            return result;
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<TKey> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));


        public async Task Insert(TEntry entry)
        {
            await Repository.Insert(entry);
            await Cache.SetAsync(entry.ID.ToString(), entry, Duration);
        }

        public Task Insert(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Insert(entry)));

        public async Task Update(TEntry entry)
        {
            await Repository.Update(entry);
            await Cache.SetAsync(entry.ID.ToString(), entry, Duration);
        }

        public Task Update(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Update(entry)));

        public async Task Delete(TKey key)
        {
            await Repository.Delete(key);
            await Cache.RemoveAsync(key.ToString());
        }

        public async Task Delete(TEntry entry)
        {
            await Repository.Delete(entry);
            await Cache.RemoveAsync(entry.ID.ToString());
        }

        public Task Delete(IEnumerable<TKey> keys)
            => Task.WhenAll(keys.Select(key => Delete(key)));

        public Task Delete(IEnumerable<TEntry> entries)
            => Task.WhenAll(entries.Select(entry => Delete(entry)));
    }
}
