using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public interface IAccountRepository
        : IRepositoryReader<int, AccountEntity>
        , IRepositoryWriter<int, AccountEntity>
    {
        Task<AccountEntity> RetrieveByUsername(string username);
    }
}
