using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Accounts;

public interface IAccountWorldRepository : IRepository<int, IAccountWorld>
{
    Task<IAccountWorld?> RetrieveByAccountAndWorld(int accountID, int worldID);
}
