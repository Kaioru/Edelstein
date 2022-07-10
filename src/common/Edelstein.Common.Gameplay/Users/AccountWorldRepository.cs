﻿using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;
using Foundatio.Caching;

namespace Edelstein.Common.Gameplay.Users
{
    public class AccountWorldRepository : CachedRepository<int, AccountWorld>, IAccountWorldRepository
    {
        public static readonly string CacheScope = "accountworlds";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public AccountWorldRepository(
            ICacheClient cache,
            IDataStore store
        ) : base(
            new ScopedCacheClient(cache, CacheScope),
            new DataStoreRepository<AccountWorld>(store),
            CacheDuration
        )
        => _store = store;

        public Task<AccountWorld> RetrieveByAccountAndWorld(int account, int world)
        {
            using var session = _store.StartSession();
            return session
                .Query<AccountWorld>()
                .Where(a => a.AccountID == account && a.WorldID == world)
                .FirstOrDefault();
        }
    }
}
