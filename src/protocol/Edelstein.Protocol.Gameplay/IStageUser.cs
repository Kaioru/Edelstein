using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser<TStage, TStageUser> : ISocketAdapter
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
}
