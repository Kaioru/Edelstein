using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public interface IAccountWorldRepository : IRepository<int, AccountWorld>
    {
        Task<AccountWorld> RetrieveByAccountAndWorld(int account, int world);
    }
}
