using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social.Guilds
{
    public interface IGuildRepository : IRepository<int, GuildRecord>
    {
        Task<GuildRecord> RetrieveByName(string name);
        Task<GuildRecord> RetrieveByMember(int member);
    }
}
