using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets;

public record StructuredSendPacket(
    [property: FieldOrder(0)] 
    short Operation
) : StructuredBasePacket;
