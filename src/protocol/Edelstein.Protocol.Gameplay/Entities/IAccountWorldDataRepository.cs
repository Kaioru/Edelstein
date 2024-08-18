using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public interface IAccountWorldDataRepository : IQueriedRepository<int, AccountWorldData>
{
    Task<AccountWorldData?> RetrieveByAccountAndWorld(int accountID, int worldID);
}
