using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay;

public interface IStage<in TStageSystemUser, out TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem> 
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
{
    Task Enter(TStageSystemUser user);
    Task Leave(TStageSystemUser user);
}
