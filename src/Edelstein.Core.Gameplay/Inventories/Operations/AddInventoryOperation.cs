using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public class AddInventoryOperation : AbstractModifyInventoryOperation
    {
        protected override ModifyInventoryOperationType Type => ModifyInventoryOperationType.Add;
        private readonly ItemSlot _item;

        public AddInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            ItemSlot item
        ) : base(inventory, slot)
        {
            _item = item;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            _item.Encode(packet);
        }
    }
}