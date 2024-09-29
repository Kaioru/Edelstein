using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserPortalScriptRequest : StructuredRecvPacket
{
    [FieldOrder(0)] public byte FieldKey { get; init; }
    
    [FieldOrder(1)] public required LPString Name { get; init; }
    
    [FieldOrder(2)] public short X { get; init; }
    [FieldOrder(3)] public short Y { get; init; }
}
