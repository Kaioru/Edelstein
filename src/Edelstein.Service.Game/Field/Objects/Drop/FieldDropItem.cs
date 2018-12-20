using System.Threading.Tasks;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Field.Objects.Drop
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
            await Field.Leave(this, () => GetLeaveFieldPacket(0x2, user));
            await user.ModifyInventory(i => i.Add(_item), true);
        }
    }
}