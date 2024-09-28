using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record ReactorLeaveField() : StructuredSendPacket((short)PacketSendOperation.ReactorLeaveField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required byte State { get; init; }
    [FieldOrder(2)] public short X { get; init; }
    [FieldOrder(3)] public short Y { get; init; }
}
