using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Models.Accounts;

public interface IAccountWorldRepository : IQueriedRepository<int, IAccountWorld>
{
    Task<IAccountWorld?> RetrieveByAccountAndWorld(int accountID, int worldID);
}
