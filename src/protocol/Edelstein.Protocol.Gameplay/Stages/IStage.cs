using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStage<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        TUser GetUser(int id);
        IEnumerable<TUser> GetUsers();

        Task Enter(TUser user);
        Task Leave(TUser user);
    }
}
