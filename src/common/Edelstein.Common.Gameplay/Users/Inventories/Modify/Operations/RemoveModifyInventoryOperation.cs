using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations
{
    public class RemoveModifyInventoryOperation : AbstractModifyInventoryOperation
    {
        public override ModifyInventoryOperations Operation => ModifyInventoryOperations.Remove;

        public RemoveModifyInventoryOperation(
            ItemInventoryType type,
            short slot
        )
        {
            Type = type;
            Slot = slot;
        }
    }
}
