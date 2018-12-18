using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Inventories.Operations
{
    public class UpdateQuantityInventoryOperation : AbstractInventoryOperation
    {
        protected override InventoryOperationType Type => InventoryOperationType.UpdateQuantity;
        private readonly short _quantity;

        public UpdateQuantityInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            short quantity
        ) : base(inventory, slot)
            => _quantity = quantity;

        public override void EncodeData(IPacket packet)
            => packet.Encode<short>(_quantity);
    }
}