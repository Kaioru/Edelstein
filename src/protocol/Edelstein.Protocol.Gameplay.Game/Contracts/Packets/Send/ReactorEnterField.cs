using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record ReactorEnterField() : StructuredSendPacket((short)PacketSendOperation.ReactorEnterField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required int TemplateID { get; init; }
    [FieldOrder(2)] public required byte State { get; init; }
    [FieldOrder(3)] public short X { get; init; }
    [FieldOrder(4)] public short Y { get; init; }
    [FieldOrder(5)] public bool Flip { get; init; }
    [FieldOrder(6)] public LPString Name { get; init; } = new();
}
