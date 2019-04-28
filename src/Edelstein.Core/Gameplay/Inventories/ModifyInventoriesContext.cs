using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
using MoreLinq;

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

        public void Set(short slot, ItemTemplate template)
        {
            Set((ItemInventoryType) (template.ID / 1000000), slot, template);
        }

        public void Set(ItemInventoryType type, short slot, ItemTemplate template)
        {
            _inventories[type].Set(slot, template);
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

        public void Move(ItemInventoryType type, short from, short to)
        {
            _inventories[type].Move(from, to);
        }

        public void Encode(IPacket packet)
        {
            _inventories.Values.ForEach(i => i.Encode(packet));
        }
    }
}