using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;

public record StructuredModifyInventoryOperationInfoAdd : StructuredModifyInventoryOperationInfo
{
    [FieldOrder(0)]
    public required StructuredItemSlot Item { get; init; }
}
