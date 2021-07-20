using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching.Serializer;
using Edelstein.Protocol.Util.Caching;
using Edelstein.Protocol.Util.Caching.Serializer;
using Microsoft.Extensions.Caching.Distributed;

namespace Edelstein.Common.Util.Caching
{
    public class Cache : ICache
    {
        private readonly IDistributedCache _cache;
        private readonly ICacheSerializer _serializer;

        public Cache(IDistributedCache cache, ICacheSerializer serializer = null)
        {
            _cache = cache;
            _serializer = serializer ?? new JsonCacheSerializer();
        }

        public Task<string> Get(string key) => _cache.GetStringAsync(key);

        public async Task<T> Get<T>(string key)
        {
            var value = await Get(key);
            if (value == null) return default;
            var result = await _serializer.Deserialize<T>(value);
            return result;
        }

        public Task Set(string key, string value, TimeSpan duration)
            => _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });

        public Task Set(string key, string value, DateTime date)
            => _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = date
            });

        public async Task Set<T>(string key, T value, TimeSpan duration)
            => await Set(key, await _serializer.Serialize(value), duration);

        public async Task Set<T>(string key, T value, DateTime date)
            => await Set(key, await _serializer.Serialize(value), date);

        public Task Refresh(string key) => _cache.RefreshAsync(key);
        public Task Remove(string key) => _cache.RemoveAsync(key);
    }
}
