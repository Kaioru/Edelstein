using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Foundatio.Caching;

namespace Edelstein.Common.Services.Social
{
    public class GuildRepository : CachedRepository<int, GuildRecord>
    {
        public static readonly string CacheScope = "servers";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public GuildRepository(
            ICacheClient cache,
            IDataStore store
        ) : base(
            new ScopedCacheClient(cache, CacheScope),
            new DataStoreRepository<GuildRecord>(store),
            CacheDuration
        )
        => _store = store;

        public Task<GuildRecord> RetrieveByMember(int member)
        {
            using var session = _store.StartSession();
            return session
                .Query<GuildRecord>()
                .Where(p => p.Members.Where(m => m.ID == member).Any())
                .FirstOrDefault();
        }
    }
}
