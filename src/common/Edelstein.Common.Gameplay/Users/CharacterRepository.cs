using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;
using Foundatio.Caching;

namespace Edelstein.Common.Gameplay.Users
{
    public class CharacterRepository : CachedRepository<int, Character>, ICharacterRepository
    {
        public static readonly string CacheScope = "characters";
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(3);

        private readonly IDataStore _store;

        public CharacterRepository(
            ICacheClient cache,
            IDataStore store
        ) : base(
            new ScopedCacheClient(cache, CacheScope),
            new DataStoreRepository<Character>(store),
            CacheDuration
        )
        => _store = store;

        public async Task<bool> CheckExistsByName(string name)
        {
            using var session = _store.StartSession();
            return (await session
                .Query<Character>()
                .Where(c => c.Name.ToLower() == name.ToLower())
                .Count()) > 0;
        }

        public Task<IEnumerable<Character>> RetrieveAllByAccountWorld(int accountworld)
        {
            using var session = _store.StartSession();
            return session
                .Query<Character>()
                .Where(c => c.AccountWorldID == accountworld)
                .All();
        }
    }
}
