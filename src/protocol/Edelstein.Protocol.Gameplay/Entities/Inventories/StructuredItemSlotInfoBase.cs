using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlotInfoBase : StructuredBasePacket
{
    [FieldOrder(0)]
    public required int ItemID { get; init; }

    [FieldOrder(1)]
    public bool HasCashItemSN => CashItemSN != null;
    
    [FieldOrder(2)]
    public long? CashItemSN { get; init; }

    [FieldOrder(3)] 
    public FDateTime DateExpire { get; init; } = new();
}
