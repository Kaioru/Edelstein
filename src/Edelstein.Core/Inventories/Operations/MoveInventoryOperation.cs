using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Inventories.Operations
{
    public class MoveInventoryOperation : AbstractInventoryOperation
    {
        protected override InventoryOperationType Type => InventoryOperationType.Move;
        private readonly short _newSlot;

        public MoveInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            short newSlot
        ) : base(inventory, slot)
            => _newSlot = newSlot;

        public override void EncodeData(IPacket packet)
            => packet.Encode<short>(_newSlot);
    }
}