using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredSendPacket(
    [property: FieldOrder(0)] 
    PacketSendOperation Operation
) : StructuredBasePacket;
