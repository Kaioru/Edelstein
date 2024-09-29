using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record ReactorChangeState() : StructuredSendPacket((short)PacketSendOperation.ReactorChangeState)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required byte State { get; init; }
    [FieldOrder(2)] public short X { get; init; }
    [FieldOrder(3)] public short Y { get; init; }
    [FieldOrder(4)] public short Delay { get; init; }
    [FieldOrder(5)] public byte ProperEventIDx { get; init; }
    [FieldOrder(6)] public byte StateEnd { get; init; }
}
