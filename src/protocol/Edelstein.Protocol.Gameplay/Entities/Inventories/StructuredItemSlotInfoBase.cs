using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlotInfoBase : StructuredBasePacket
{
    [FieldOrder(0)]
    public required int ItemID { get; init; }

    [FieldOrder(1)] 
    public BPNullable<long> CashItemSN { get; init; } = new();

    [FieldOrder(2)] 
    public FDateTime DateExpire { get; init; } = new();
}
