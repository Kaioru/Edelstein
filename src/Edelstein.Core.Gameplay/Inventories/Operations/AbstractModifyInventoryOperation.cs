using Edelstein.Entities.Inventories;
using Edelstein.Network.Packets;

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

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) _inventory);
            packet.Encode<short>(Slot);

            EncodeData(packet);
        }

        protected virtual void EncodeData(IPacket packet)
        {
        }
    }
}