using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public record PipedPacketMessage<TStageSystemUser, TPacket>(
    TStageSystemUser User,
    TPacket Packet
)
    where TPacket : StructuredBasePacket;
