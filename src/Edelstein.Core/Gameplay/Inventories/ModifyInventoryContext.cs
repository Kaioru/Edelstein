using System.Collections.Generic;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoryContext
    {
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;
        private readonly Queue<AbstractModifyInventoryOperation> _operations;

        public ModifyInventoryContext(ItemInventoryType type, ItemInventory inventory)
        {
            _type = type;
            _inventory = inventory;
            _operations = new Queue<AbstractModifyInventoryOperation>();
        }

        public void Set(short slot, ItemTemplate template)
        {
            Set(slot, template.ToItemSlot());
        }

        public void Set(short slot, ItemSlot item)
        {
            if (_inventory.Items.ContainsKey(slot))
                Remove(slot);

            _inventory.Items.Add(slot, item);
            _operations.Enqueue(new AddInventoryOperation(_type, slot, item));
        }

        public void Remove(short slot)
        {
            _inventory.Items.Remove(slot);
            _operations.Enqueue(new RemoveInventoryOperation(_type, slot));
        }
    }
}