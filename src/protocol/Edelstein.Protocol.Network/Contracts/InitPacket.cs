using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Network.Contracts;

public record InitPacket : StructuredBasePacket
{
    [FieldOrder(0)] public required short Version { get; init; }
    [FieldOrder(1)] public required LPString Patch { get; init; } 
    
    [FieldOrder(2)] public required uint SeqRecv { get; init; }
    [FieldOrder(3)] public required uint SeqSend { get; init; }
    
    [FieldOrder(4)] public required byte Locale { get; init; }
}
