using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify
{
    public class ModifyMultiInventoryContext : IModifyMultiInventoryContext, IPacketWritable
    {
        public IModifyInventoryContext this[ItemInventoryType key] => _contexts[key];
        public IEnumerable<IModifyInventoryOperation> History => _contexts.Values.SelectMany(v => v.History).ToList();

        private readonly IDictionary<ItemInventoryType, ModifyInventoryContext> _contexts;

        public ModifyMultiInventoryContext(
            IDictionary<ItemInventoryType, ItemInventory> inventories,
            ITemplateRepository<ItemTemplate> templates
        )
        {
            _contexts = inventories.ToDictionary(
                kv => kv.Key,
                kv => new ModifyInventoryContext(kv.Key, kv.Value, templates)
            );
        }

        public void Set(BodyPart part, ItemSlotEquip equip) => this[ItemInventoryType.Equip].Set((short)-(short)part, equip);
        public void Set(BodyPart part, int templateID) => this[ItemInventoryType.Equip].Set((short)-(short)part, templateID);
        public void Set(BodyPart part, ItemEquipTemplate template) => this[ItemInventoryType.Equip].Set((short)-(short)part, template);

        public void Add(AbstractItemSlot item) => this[(ItemInventoryType)(item.TemplateID / 1_000_000)].Add(item);
        public void Add(int templateID, short quantity = 1) => this[(ItemInventoryType)(templateID / 1_000_000)].Add(templateID, quantity);
        public void Add(ItemTemplate template, short quantity = 1) => this[(ItemInventoryType)(template.ID / 1_000_000)].Add(template, quantity);

        public void Remove(int templateID, short count) => this[(ItemInventoryType)(templateID / 1_000_000)].Remove(templateID, count);
        public void Remove(AbstractItemSlot item) => this[(ItemInventoryType)(item.TemplateID / 1_000_000)].Remove(item);
        public void Remove(AbstractItemSlot item, short count) => this[(ItemInventoryType)(item.TemplateID / 1_000_000)].Remove(item, count);

        public void Update(AbstractItemSlot item) => this[(ItemInventoryType)(item.TemplateID / 1_000_000)].Update(item);

        public void WriteToPacket(IPacketWriter writer)
        {
            var history = History
                .OfType<AbstractModifyInventoryOperation>()
                .OrderBy(o => o.Timestamp)
                .ToList();

            writer.WriteByte((byte)history.Count);
            history.ForEach(o => writer.Write(o));
        }
    }
}
