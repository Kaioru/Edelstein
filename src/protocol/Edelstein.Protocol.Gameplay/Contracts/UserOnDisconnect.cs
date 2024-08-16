namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnDisconnect<TStageSystem, TStageSystemUser>(
    TStageSystemUser User
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>;
