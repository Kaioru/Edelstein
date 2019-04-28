using System.Threading.Tasks;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Fields.Objects.Drops
{
    public class ItemFieldDrop : AbstractFieldDrop
    {
        public override bool IsMoney => false;
        public override int Info => _item.TemplateID;

        private readonly ItemSlot _item;

        public ItemFieldDrop(ItemSlot item)
            => _item = item;

        public override async Task PickUp(FieldUser user)
        {
            await Field.Leave(this, () => GetLeaveFieldPacket(0x2, user));
            await user.ModifyInventory(i => i.Add(_item), true);
        }
    }
}