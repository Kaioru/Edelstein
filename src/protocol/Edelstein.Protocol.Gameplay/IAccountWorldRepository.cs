using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public interface IAccountWorldRepository : IRepository<int, AccountWorldEntity>
    {
        Task<AccountWorldEntity> RetrieveByAccountAndWorld(int account, int world);
    }
}
