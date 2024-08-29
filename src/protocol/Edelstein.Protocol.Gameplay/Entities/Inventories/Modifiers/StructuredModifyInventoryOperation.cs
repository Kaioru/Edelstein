using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public record StructuredModifyInventoryOperation: StructuredBasePacket
{
    [FieldOrder(0)]
    public required ModifyInventoryOperationType Type { get; init; }
    
    [FieldOrder(1)]
    [Subtype(nameof(Type), ModifyInventoryOperationType.Add, typeof(StructuredModifyInventoryOperationInfoAdd))]
    [Subtype(nameof(Type), ModifyInventoryOperationType.UpdateNumber, typeof(StructuredModifyInventoryOperationInfoUpdateNumber))]
    [Subtype(nameof(Type), ModifyInventoryOperationType.Move, typeof(StructuredModifyInventoryOperationInfoMove))]
    [Subtype(nameof(Type), ModifyInventoryOperationType.Remove, typeof(StructuredModifyInventoryOperationInfoRemove))]
    [Subtype(nameof(Type), ModifyInventoryOperationType.UpdateEXP, typeof(StructuredModifyInventoryOperationInfoUpdateEXP))]
    [SubtypeDefault(typeof(StructuredModifyInventoryOperationInfo))]
    public required StructuredModifyInventoryOperationInfo Info { get; init; }
}
