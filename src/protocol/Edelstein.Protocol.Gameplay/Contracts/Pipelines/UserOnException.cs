namespace Edelstein.Protocol.Gameplay.Contracts.Pipelines;

public record UserOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) where TStageUser : IStageUser<TStageUser>;
