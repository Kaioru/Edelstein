using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystem<TStageSystem, TStageSystemUser> :
    ISocketUserAdapter<TStageSystemUser>,
    ISocketUserInitializer<TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    string ID { get; }
}
