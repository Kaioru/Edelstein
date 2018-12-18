using Edelstein.Data.Entities.Inventory;

namespace Edelstein.Core.Inventories.Operations
{
    public class RemoveInventoryOperation : AbstractInventoryOperation
    {
        protected override InventoryOperationType Type => InventoryOperationType.Remove;

        public RemoveInventoryOperation(
            ItemInventoryType inventory,
            short slot
        ) : base(inventory, slot)
        {
        }
    }
}