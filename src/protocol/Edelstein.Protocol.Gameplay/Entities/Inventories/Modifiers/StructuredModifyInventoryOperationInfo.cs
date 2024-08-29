using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public record StructuredModifyInventoryOperationInfo : StructuredBasePacket
{
    [FieldOrder(0)]
    public required ItemInventoryType Inventory { get; init; }
    
    [FieldOrder(1)]
    public required short Slot { get; init; }
}
