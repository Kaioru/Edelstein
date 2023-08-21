namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacketAliveAck<TStageUser>(
    TStageUser User,
    DateTime Date
) where TStageUser : IStageUser<TStageUser>;
