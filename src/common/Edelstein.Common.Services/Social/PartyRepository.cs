using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Foundatio.Caching;

namespace Edelstein.Common.Services.Social
{
    public class PartyRepository : CachedRepository<int, PartyRecord>
    {
        public static readonly string CacheScope = "servers";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public PartyRepository(
            ICacheClient cache,
            IDataStore store
        ) : base(
            new ScopedCacheClient(cache, CacheScope),
            new DataStoreRepository<PartyRecord>(store),
            CacheDuration
        )
        => _store = store;

        public async Task<PartyRecord> RetrieveByMember(int member)
        {
            using var session = _store.StartSession();

            return await session
                .Query<PartyRecord>()
                .Where(p => p.Members.Any(m => m.ID == member))
                .FirstOrDefault();
        }
    }
}
