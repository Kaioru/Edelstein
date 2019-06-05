using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Provider.Templates.Item;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields.Inventories
{
    public class FieldUserInventorySpeaker : Speaker
    {
        private readonly FieldUser _fieldUser;

        public FieldUserInventorySpeaker(IConversationContext context, FieldUser fieldUser) : base(context)
        {
            _fieldUser = fieldUser;
        }

        public void Add(int templateID, short quantity = 1, ItemVariationType type = ItemVariationType.None)
        {
            var template = _fieldUser.Service.TemplateManager
                .Get<ItemTemplate>(templateID);
            _fieldUser.ModifyInventory(i => i.Add(template, quantity)).Wait();
        }

        public void Remove(int templateID, short quantity = 1)
            => _fieldUser.ModifyInventory(i => i.Remove(templateID, quantity)).Wait();

        public byte GetInventoryLimit(ItemInventoryType type)
            => (byte) _fieldUser.Character.Inventories[type].SlotMax;

        public void SetInventoryLimit(ItemInventoryType type, byte slotMax)
            => _fieldUser.ModifyInventoryLimit(type, slotMax).Wait();

        public int SlotCount(ItemInventoryType inventory) => _fieldUser.Character.AvailableSlotsFor(inventory);
        public int ItemCount(int templateID) => _fieldUser.Character.GetItemCount(templateID);

        public bool HasSlotFor(IEnumerable<int> templates) => templates
            .GroupBy(i => (ItemInventoryType) (i / 1000000))
            .All(g => SlotCount(g.Key) >= g.Count());

        public bool HasSlotFor(int templateID) => SlotCount((ItemInventoryType) (templateID / 1000000)) > 0;
    }
}