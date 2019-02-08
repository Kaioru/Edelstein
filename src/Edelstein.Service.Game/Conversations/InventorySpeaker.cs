using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Provider.Templates.Item;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Conversations
{
    public class InventorySpeaker : Speaker
    {
        private FieldUser _fieldUser;

        public InventorySpeaker(
            IConversationContext context,
            FieldUser fieldUser,
            int templateID = 9010000,
            ScriptMessageParam param = (ScriptMessageParam) 0
        ) : base(context, templateID, param)
            => _fieldUser = fieldUser;

        public void Add(int templateID, short quantity = 1, ItemVariationType type = ItemVariationType.None)
        {
            var template = _fieldUser.Socket.WvsGame.TemplateManager
                .Get<ItemTemplate>(templateID);
            _fieldUser.ModifyInventory(i => i.Add(template, quantity, type)).Wait();
        }

        public void Remove(int templateID, short quantity = 1)
            => _fieldUser.ModifyInventory(i => i.Remove(templateID, quantity)).Wait();

        public int SlotCount(ItemInventoryType inventory) => _fieldUser.Character.AvailableSlotsFor(inventory);
        public int ItemCount(int templateID) => _fieldUser.Character.GetItemCount(templateID);

        public bool HasSlotFor(IEnumerable<int> templates) => templates
            .GroupBy(i => (ItemInventoryType) (i / 1000000))
            .All(g => SlotCount(g.Key) >= g.Count());

        public bool HasSlotFor(int templateID) => SlotCount((ItemInventoryType) (templateID / 1000000)) > 0;
    }
}