using Edelstein.Common.Caching.Serializer;
using Edelstein.Protocol.Caching;
using Edelstein.Protocol.Caching.Serializer;
using Microsoft.Extensions.Caching.Distributed;

namespace Edelstein.Common.Caching
{
    public class CacheFactory : ICacheFactory
    {
        private readonly IDistributedCache _cache;
        private readonly ICacheSerializer _serializer;

        public CacheFactory(IDistributedCache cache, ICacheSerializer serializer = null)
        {
            _cache = cache;
            _serializer = serializer ?? new JsonCacheSerializer();
        }

        public ICache CreateCache() => new Cache(_cache, _serializer);
        public ICache CreateScopedCache(string scope) => new ScopedCache(scope, CreateCache());
    }
}
