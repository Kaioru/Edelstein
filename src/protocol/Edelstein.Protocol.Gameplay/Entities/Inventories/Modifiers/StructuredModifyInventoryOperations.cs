using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public record StructuredModifyInventoryOperations : StructuredBasePacket
{
    [FieldOrder(0)]
    public byte Count { get; init; }
    
    [FieldOrder(1)]
    [FieldCount(nameof(Count))]
    public required List<StructuredModifyInventoryOperation> Operations { get; init; }
}
