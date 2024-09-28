using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay;

public interface IStage<out TStageSystem, in TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    Task Enter(TStageSystemUser user);
    Task Leave(TStageSystemUser user);
}
