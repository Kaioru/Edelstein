using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Social.Parties
{
    public interface IPartyRepository : IRepository<int, PartyRecord>
    {
        Task<PartyRecord> RetrieveByMember(int member);
    }
}
