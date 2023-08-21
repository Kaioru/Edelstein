namespace Edelstein.Protocol.Gameplay.Contracts.Pipelines;

public record UserOnAliveAck<TStageUser>(
    TStageUser User,
    DateTime Date
) where TStageUser : IStageUser<TStageUser>;
