using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public interface IAccountRepository : IQueriedRepository<int, IAccount>
{
    Task<IAccount?> RetrieveByUsername(string username);
}
