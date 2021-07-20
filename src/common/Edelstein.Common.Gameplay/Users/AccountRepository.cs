using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users;

namespace Edelstein.Common.Gameplay.Users
{
    public class AccountRepository : DataStoreRepository<Account>, IAccountRepository
    {
        public AccountRepository(IDataStore store) : base(store) { }

        public Task<Account> RetrieveByUsername(string username)
            => Store.StartSession()
                    .Query<Account>()
                    .Where(a => a.Username == username)
                    .FirstOrDefault();
    }
}
