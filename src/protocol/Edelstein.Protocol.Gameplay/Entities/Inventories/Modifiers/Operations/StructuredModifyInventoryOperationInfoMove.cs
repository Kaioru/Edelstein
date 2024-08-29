using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;

public record StructuredModifyInventoryOperationInfoMove : StructuredModifyInventoryOperationInfo
{
    [FieldOrder(0)]
    public required short ToSlot { get; init; }
}
