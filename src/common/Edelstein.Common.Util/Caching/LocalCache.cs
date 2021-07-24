using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Edelstein.Common.Util.Caching
{
    public class LocalCache : ICache
    {
        private readonly IMemoryCache _cache;

        public LocalCache()
            => _cache = new MemoryCache(new MemoryCacheOptions());

        public Task<string> Get(string key) => Task.FromResult(_cache.Get(key).ToString());
        public Task<T> Get<T>(string key) => Task.FromResult(_cache.Get<T>(key));

        public Task Set(string key, string value, TimeSpan duration)
        {
            _cache.Set(key, value, duration);
            return Task.CompletedTask;
        }

        public Task Set(string key, string value, DateTime date)
        {
            _cache.Set(key, value, date);
            return Task.CompletedTask;
        }

        public Task Set<T>(string key, T value, TimeSpan duration)
        {
            _cache.Set(key, value, duration);
            return Task.CompletedTask;
        }

        public Task Set<T>(string key, T value, DateTime date)
        {
            _cache.Set(key, value, date);
            return Task.CompletedTask;
        }

        public async Task Refresh(string key, TimeSpan duration)
            => await Set(key, _cache.Get(key), duration);

        public async Task Refresh(string key, DateTime date)
            => await Set(key, _cache.Get(key), date);

        public Task Remove(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
