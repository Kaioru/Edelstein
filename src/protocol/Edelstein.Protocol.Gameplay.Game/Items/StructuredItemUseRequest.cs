using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public record StructuredItemUseRequest : StructuredRecvPacket
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    [FieldOrder(1)] public short Pos { get; init; }
    [FieldOrder(2)] public int ItemID { get; init; }
}
