using System.Threading.Tasks;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Fields.Objects.Drop
{
    public class FieldDropItem : AbstractFieldDrop
    {
        public override bool IsMoney => false;
        public override int Info => _item.TemplateID;

        private readonly ItemSlot _item;

        public FieldDropItem(ItemSlot item)
            => _item = item;

        public override async Task PickUp(FieldUser user)
        {
            await user.ModifyInventory(i => i.Add(_item), true);
            await Field.Leave(this, () => GetLeaveFieldPacket(0x2, user));
        }
    }
}