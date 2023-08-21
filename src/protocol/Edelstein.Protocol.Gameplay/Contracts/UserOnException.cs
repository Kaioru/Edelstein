namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) where TStageUser : IStageUser<TStageUser>;
