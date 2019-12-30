using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Core.Templates.Items;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoriesContext : IModifyInventoriesContext
    {
        public IModifyInventoryContext this[ItemInventoryType key] => _inventories[key];
        private readonly IDictionary<ItemInventoryType, ModifyInventoryContext> _inventories;

        public IEnumerable<AbstractModifyInventoryOperation> Operations =>
            _inventories.Values.SelectMany(i => i.Operations);

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
        {
            var operations = _inventories.Values.SelectMany(v => v.Operations).ToList();

            packet.Encode<byte>((byte) operations.Count);
            operations.ForEach(o => o.Encode(packet));
        }
    }
}