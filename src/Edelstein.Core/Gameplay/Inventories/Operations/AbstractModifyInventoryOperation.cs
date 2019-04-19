using Edelstein.Database.Inventories;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public abstract class AbstractModifyInventoryOperation
    {
        protected abstract ModifyInventoryOperationType Type { get; }
        private readonly ItemInventoryType _inventory;
        private readonly short _slot;

        public AbstractModifyInventoryOperation(
            ItemInventoryType inventory,
            short slot)
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

        protected virtual void EncodeData(IPacket packet)
        {
        }
    }
}