using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;

public class RemoveModifyInventoryOperation : AbstractModifyInventoryOperation
{
    public RemoveModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot
    ) : base(ModifyInventoryOperationType.Remove, inventory, slot)
    {
    }
}
