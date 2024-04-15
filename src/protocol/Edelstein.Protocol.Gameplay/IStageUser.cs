using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser<TStageUser, out TStageSystem> : ISocketUser
    where TStageSystem : IStageSystem<TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem>
{
    TStageSystem System { get; }
}
