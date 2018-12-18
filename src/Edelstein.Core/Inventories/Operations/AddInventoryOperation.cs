using Edelstein.Core.Extensions;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Inventories.Operations
{
    public class AddInventoryOperation : AbstractInventoryOperation
    {
        protected override InventoryOperationType Type => InventoryOperationType.Add;
        private readonly ItemSlot _item;

        public AddInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            ItemSlot item
        ) : base(inventory, slot)
            => _item = item;

        public override void EncodeData(IPacket packet)
        {
            switch (_item)
            {
                case ItemSlotEquip equip:
                    equip.Encode(packet);
                    break;
                case ItemSlotBundle bundle:
                    bundle.Encode(packet);
                    break;
            }
        }
    }
}