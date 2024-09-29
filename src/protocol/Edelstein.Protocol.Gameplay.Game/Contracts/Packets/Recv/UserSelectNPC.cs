using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserSelectNPC : StructuredRecvPacket
{
    [FieldOrder(0)] public int ObjectID { get; init; }
    
    [FieldOrder(1)] public short X { get; init; }
    [FieldOrder(2)] public short Y { get; init; }
}
