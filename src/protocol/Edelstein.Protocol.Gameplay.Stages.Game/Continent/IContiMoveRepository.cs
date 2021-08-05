using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public interface IContiMoveRepository : ILocalRepository<int, IContiMove>
    {
        Task<IContiMove> RetrieveByInfo(string info);
        Task<IContiMove> RetrieveByField(IField field);
    }
}
