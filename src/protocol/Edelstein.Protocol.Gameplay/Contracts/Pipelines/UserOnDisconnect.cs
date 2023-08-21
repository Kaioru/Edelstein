namespace Edelstein.Protocol.Gameplay.Contracts.Pipelines;

public record UserOnDisconnect<TStageUser>(
    TStageUser User
) where TStageUser : IStageUser<TStageUser>;
