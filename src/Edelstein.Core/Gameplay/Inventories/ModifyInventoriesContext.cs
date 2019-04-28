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

        public void Add(ItemSlot item)
        {
            _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Add(item);
        }

        public void Add(ItemInventoryType type, ItemSlot item)
        {
            _inventories[type].Add(item);
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

        public void Remove(ItemInventoryType type, short slot, int count)
        {
            _inventories[type].Remove(slot, count);
        }

        public void Remove(int templateId, int count)
        {
            _inventories[(ItemInventoryType) (templateId / 1000000)].Remove(templateId, count);
        }

        public void Remove(ItemInventoryType type, int templateId, int count)
        {
            _inventories[type].Remove(templateId, count);
        }
        
        public ItemSlotBundle Take(ItemInventoryType type, short slot, short count = 1)
        {
            return _inventories[type].Take(slot, count);
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