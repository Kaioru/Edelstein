using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacket<TStageUser>(
    TStageUser User,
    IPacket Packet
) where TStageUser : IStageUser<TStageUser>;
