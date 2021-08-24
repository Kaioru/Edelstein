using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Services
{
    public class ServerRegistryRepository : CachedRepository<int, ServerRegistryRecord>
    {
        public static readonly string CacheScope = "servers";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public ServerRegistryRepository(
            ICache cache,
            IDataStore store
        ) : base(
            new ScopedCache(CacheScope, cache),
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
