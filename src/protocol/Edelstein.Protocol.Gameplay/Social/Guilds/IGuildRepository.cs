using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public interface IGuildRepository : IRepository<int, IGuild>
    {
        Task<IGuild> RetrieveByName(string name);
        Task<IGuild> RetrieveByMember(int member);
    }
}
