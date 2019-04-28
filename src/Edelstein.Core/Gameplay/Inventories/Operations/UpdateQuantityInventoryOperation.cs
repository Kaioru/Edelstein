using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public class UpdateQuantityInventoryOperation : AbstractModifyInventoryOperation
    {
        protected override ModifyInventoryOperationType Type { get; }
        private readonly short _quantity;

        public UpdateQuantityInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            short quantity
        ) : base(inventory, slot)
        {
            _quantity = quantity;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<short>(_quantity);
        }
    }
}