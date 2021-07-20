using Edelstein.Common.Util.Caching.Serializer;
using Edelstein.Protocol.Util.Caching;
using Edelstein.Protocol.Util.Caching.Serializer;
using Microsoft.Extensions.Caching.Distributed;

namespace Edelstein.Common.Util.Caching
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
