using System.Collections.Generic;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Database.Inventories;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoriesContext
    {
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;
        private readonly Queue<AbstractModifyInventoryOperation> _operations;

        public ModifyInventoriesContext(ItemInventoryType type, ItemInventory inventory)
        {
            _type = type;
            _inventory = inventory;
            _operations = new Queue<AbstractModifyInventoryOperation>();
        }
    }
}