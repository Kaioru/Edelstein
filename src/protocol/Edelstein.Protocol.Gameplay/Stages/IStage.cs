using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStage<TStage, TUser> : IPacketDispatcher
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        TUser GetUser(int id);
        IEnumerable<TUser> GetUsers();

        Task Enter(TUser user);
        Task Leave(TUser user);
    }
}
