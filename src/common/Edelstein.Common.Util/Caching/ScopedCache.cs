using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Util.Caching
{
    public class ScopedCache : ICache
    {
        private readonly string _scope;
        private readonly ICache _cache;

        public ScopedCache(string scope, ICache cache)
        {
            _scope = scope;
            _cache = cache;
        }

        public Task<string> Get(string key) => _cache.Get($"{_scope}:{key}");
        public Task<T> Get<T>(string key) => _cache.Get<T>($"{_scope}:{key}");

        public Task Set(string key, string value, TimeSpan duration) => _cache.Set($"{_scope}:{key}", value, duration);
        public Task Set(string key, string value, DateTime date) => _cache.Set($"{_scope}:{key}", value, date);
        public Task Set<T>(string key, T value, TimeSpan duration) => _cache.Set($"{_scope}:{key}", value, duration);
        public Task Set<T>(string key, T value, DateTime date) => _cache.Set($"{_scope}:{key}", value, date);

        public Task Refresh(string key, TimeSpan duration) => _cache.Refresh($"{_scope}:{key}", duration);
        public Task Refresh(string key, DateTime date) => _cache.Refresh($"{_scope}:{key}", date);

        public Task Remove(string key) => _cache.Remove($"{_scope}:{key}");
    }
}
