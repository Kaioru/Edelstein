using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Foundatio.Caching;

namespace Edelstein.Common.Services
{
    public class ServerRegistryRepository : CachedRepository<int, ServerRegistryRecord>
    {
        public static readonly string CacheScope = "servers";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public ServerRegistryRepository(
            ICacheClient cache,
            IDataStore store
        ) : base(
            new ScopedCacheClient(cache, CacheScope),
            new DataStoreRepository<ServerRegistryRecord>(store),
            CacheDuration
        )
        => _store = store;

        public Task<ServerRegistryRecord> RetrieveByServerID(string id)
        {
            using var session = _store.StartSession();
            return session
                .Query<ServerRegistryRecord>()
                .Where(s => s.ServerID == id)
                .FirstOrDefault();
        }

        public Task<IEnumerable<ServerRegistryRecord>> RetrieveAllByMetadata(IDictionary<string, string> metadata)
        {
            using var session = _store.StartSession();
            var result = session.Query<ServerRegistryRecord>();

            if (metadata.Count > 0)
                foreach (var kv in metadata)
                    result = result.Where(s =>
                        //s.Metadata.ContainsKey(kv.Key) &&
                        s.Metadata[kv.Key] == kv.Value
                    );

            return result.All();
        }
    }
}
