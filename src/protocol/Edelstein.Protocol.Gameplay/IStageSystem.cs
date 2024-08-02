using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystem<TStageSystemUser, TStageSystem> :
    ISocketUserAdapter<TStageSystemUser>,
    ISocketUserInitializer<TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
{
    string ID { get; }
}
