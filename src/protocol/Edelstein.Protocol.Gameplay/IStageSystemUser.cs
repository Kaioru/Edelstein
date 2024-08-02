using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystemUser<TStageSystemUser, out TStageSystem> : 
    ISocketUser
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem> 
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
{
    TStageSystem System { get; }
}
