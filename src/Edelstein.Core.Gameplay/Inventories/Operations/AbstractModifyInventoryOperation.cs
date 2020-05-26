using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public abstract class AbstractModifyInventoryOperation
    {
        protected abstract ModifyInventoryOperationType Type { get; }
        private readonly ItemInventoryType _inventory;
        public short Slot { get; }

        protected AbstractModifyInventoryOperation(
            ItemInventoryType inventory,
            short slot)
        {
            _inventory = inventory;
            Slot = slot;
        }

        public void Encode(IPacketEncoder packet)
        {
            packet.EncodeByte((byte) Type);
            packet.EncodeByte((byte) _inventory);
            packet.EncodeShort(Slot);

            EncodeData(packet);
        }

        protected virtual void EncodeData(IPacketEncoder packet)
        {
        }
    }
}