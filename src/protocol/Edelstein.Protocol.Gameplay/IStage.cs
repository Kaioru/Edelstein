using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay;

public interface IStage<in TStageUser, out TStageSystem>
    where TStageSystem : IStageSystem<TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem>
{
    Task Enter(TStageUser user);
    Task Leave(TStageUser user);
}
