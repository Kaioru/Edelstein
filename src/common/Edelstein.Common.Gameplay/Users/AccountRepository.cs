using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Gameplay.Users
{
    public class AccountRepository : CachedRepository<int, Account>, IAccountRepository
    {
        public static readonly string CacheScope = "accounts";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public AccountRepository(
            ICache cache,
            IDataStore store
        ) : base(
            new ScopedCache(CacheScope, cache),
            new DataStoreRepository<Account>(store),
            CacheDuration
        )
        => _store = store;

        public Task<Account> RetrieveByUsername(string username)
        {
            using var session = _store.StartSession();
            return session
                .Query<Account>()
                .Where(a => a.Username == username)
                .FirstOrDefault();
        }
    }
}
