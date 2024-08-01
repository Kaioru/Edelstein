using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredRecvPacket : StructuredBasePacket
{
    [FieldOrder(0)]
    public PacketRecvOperation Operation { get; init; }
}
