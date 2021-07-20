using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;

namespace Edelstein.Common.Gameplay.Users
{
    public class AccountWorldRepository : DataStoreRepository<AccountWorld>, IAccountWorldRepository
    {
        public AccountWorldRepository(IDataStore store) : base(store) { }

        public Task<AccountWorld> RetrieveByAccountAndWorld(int account, int world)
            => Store.StartSession()
                    .Query<AccountWorld>()
                    .Where(a => a.AccountID == account && a.WorldID == world)
                    .FirstOrDefault();
    }
}
