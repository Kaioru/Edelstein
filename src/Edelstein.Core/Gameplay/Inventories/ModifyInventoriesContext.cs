using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Inventories;
using Edelstein.Database.Inventories.Items;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoriesContext
    {
        private readonly IDictionary<ItemInventoryType, ModifyInventoryContext> _inventories;

        public ModifyInventoriesContext(IDictionary<ItemInventoryType, ItemInventory> inventories)
        {
            _inventories = inventories.ToDictionary(
                kv => kv.Key,
                kv => new ModifyInventoryContext(kv.Key, kv.Value)
            );
        }

        public void Set(short slot, ItemSlot item)
        {
            Set((ItemInventoryType) (item.TemplateID / 1000000), slot, item);
        }

        public void Set(ItemInventoryType type, short slot, ItemSlot item)
        {
            _inventories[type].Set(slot, item);
        }

        public void Remove(ItemInventoryType type, short slot)
        {
            _inventories[type].Remove(slot);
        }
    }
}