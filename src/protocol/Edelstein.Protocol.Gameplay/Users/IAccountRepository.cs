using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public interface IAccountRepository : IRepository<int, Account>
    {
        Task<Account> RetrieveByUsername(string username);
    }
}
