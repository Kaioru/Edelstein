using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;

public record StructuredModifyInventoryOperationInfoUpdateEXP : StructuredModifyInventoryOperationInfo
{
    [FieldOrder(0)]
    public required int EXP { get; init; }
}
