using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Game.Handling;

public record PipedFieldPacketMessage<TPacket>(
    IFieldUser User,
    TPacket Packet
)
    where TPacket : StructuredBasePacket;
