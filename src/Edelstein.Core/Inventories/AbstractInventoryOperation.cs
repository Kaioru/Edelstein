using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Inventories
{
    public abstract class AbstractInventoryOperation
    {
        protected abstract InventoryOperationType Type { get; }
        private readonly ItemInventoryType _inventory;
        private readonly short _slot;

        public AbstractInventoryOperation(ItemInventoryType inventory, short slot)
        {
            _inventory = inventory;
            _slot = slot;
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) _inventory);
            packet.Encode<short>(_slot);

            EncodeData(packet);
        }

        public virtual void EncodeData(IPacket packet)
        {
        }
    }
}