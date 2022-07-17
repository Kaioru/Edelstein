using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Accounts;

public interface IAccountRepository : IRepository<int, IAccount>
{
    Task<IAccount?> RetrieveByUsername(string username);
}
