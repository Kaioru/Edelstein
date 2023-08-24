namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnDisconnect<TStageUser>(
    TStageUser User
) where TStageUser : IStageUser<TStageUser>;
