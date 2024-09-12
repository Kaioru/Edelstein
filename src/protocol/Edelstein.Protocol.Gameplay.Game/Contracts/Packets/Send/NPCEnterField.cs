using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record NPCEnterField() : StructuredSendPacket((short)PacketSendOperation.NpcEnterField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required int TemplateID { get; init; }

    [FieldOrder(2)] public short X { get; init; }
    [FieldOrder(3)] public short Y { get; init; }
    [FieldOrder(4)] public byte MoveAction { get; init; }
    [FieldOrder(5)] public byte Foothold { get; init; }
    
    [FieldOrder(6)] public short RangeMin { get; init; }
    [FieldOrder(7)] public short RangeMax { get; init; }
    
    [FieldOrder(8)] public bool Enabled { get; init; }
}
