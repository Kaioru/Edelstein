using Edelstein.Database.Entities.Inventories;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public class RemoveInventoryOperation : AbstractModifyInventoryOperation
    {
        protected override ModifyInventoryOperationType Type => ModifyInventoryOperationType.Remove;

        public RemoveInventoryOperation(
            ItemInventoryType inventory,
            short slot
        ) : base(inventory, slot)
        {
        }
    }
}