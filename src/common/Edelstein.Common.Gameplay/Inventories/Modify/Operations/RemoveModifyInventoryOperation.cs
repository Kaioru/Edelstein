using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Common.Gameplay.Inventories.Modify.Operations;

public class RemoveModifyInventoryOperation : AbstractModifyInventoryOperation
{
    public RemoveModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot
    ) : base(ModifyInventoryOperationType.Remove, inventory, slot)
    {
    }
}
