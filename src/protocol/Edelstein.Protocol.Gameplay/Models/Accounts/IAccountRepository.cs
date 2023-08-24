using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Models.Accounts;

public interface IAccountRepository : IQueriedRepository<int, IAccount>
{
    Task<IAccount?> RetrieveByUsername(string username);
}
