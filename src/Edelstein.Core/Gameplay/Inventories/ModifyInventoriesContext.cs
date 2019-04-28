using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoriesContext : IModifyInventoriesContext
    {
        public IModifyInventoryContext this[ItemInventoryType key] => _inventories[key];
        private readonly IDictionary<ItemInventoryType, ModifyInventoryContext> _inventories;

        public ModifyInventoriesContext(IDictionary<ItemInventoryType, ItemInventory> inventories)
        {
            _inventories = inventories.ToDictionary(
                kv => kv.Key,
                kv => new ModifyInventoryContext(kv.Key, kv.Value)
            );
        }

        public void Add(ItemSlot item)
            => _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Add(item);

        public void Add(ItemTemplate template, short quantity = 1)
            => _inventories[(ItemInventoryType) (template.ID / 1000000)].Add(template, quantity);
        
        public void Set(short slot, ItemSlot item)
            => _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Set(slot, item);

        public void Set(short slot, ItemTemplate template, short quantity = 1)
            => _inventories[(ItemInventoryType) (template.ID / 1000000)].Set(slot, template, quantity);

        public void Remove(ItemSlot item)
            => _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Remove(item);

        public void Remove(ItemSlot item, short count)
            => _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Remove(item, count);

        public void Remove(int template, short count)
            => _inventories[(ItemInventoryType) (template / 1000000)].Remove(template, count);

        public ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1)
            => _inventories[(ItemInventoryType) (bundle.TemplateID / 1000000)].Take(bundle, count);

        public ItemSlotBundle Take(int template, short count = 1)
            => _inventories[(ItemInventoryType) (template / 1000000)].Take(template, count);

        public void Update(ItemSlot item)
            => _inventories[(ItemInventoryType) (item.TemplateID / 1000000)].Update(item);

        public void Encode(IPacket packet)
            => _inventories.Values.ForEach(i => i.Encode(packet));
    }
}