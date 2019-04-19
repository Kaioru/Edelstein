using Edelstein.Database.Inventories;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public class MoveInventoryOperation : AbstractModifyInventoryOperation
    {
        protected override ModifyInventoryOperationType Type => ModifyInventoryOperationType.Move;
        private readonly short _toSlot;

        public MoveInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            short toSlot
        ) : base(inventory, slot)
        {
            this._toSlot = toSlot;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<short>(_toSlot);
        }
    }
}