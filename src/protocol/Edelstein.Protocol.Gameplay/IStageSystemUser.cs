using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystemUser<out TStageSystem, TStageSystemUser> : 
    ISocketUser
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    TStageSystem System { get; }
}
