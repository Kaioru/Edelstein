using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;

public record StructuredModifyInventoryOperationInfoUpdateNumber : StructuredModifyInventoryOperationInfo
{
    [FieldOrder(0)]
    public required short Number { get; init; }
}
