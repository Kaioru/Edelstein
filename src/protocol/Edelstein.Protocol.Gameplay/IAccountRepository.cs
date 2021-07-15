using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public interface IAccountRepository : IRepository<int, AccountEntity>
    {
        Task<AccountEntity> RetrieveByUsername(string username);
    }
}
