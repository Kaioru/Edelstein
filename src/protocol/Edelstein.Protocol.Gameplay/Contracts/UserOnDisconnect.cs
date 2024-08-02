namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnDisconnect<TStageSystemUser, TStageSystem>(
    TStageSystemUser User
)
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>;
