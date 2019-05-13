using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public abstract class AbstractModifyInventoryOperation
    {
        protected abstract ModifyInventoryOperationType Type { get; }
        public ItemInventoryType Inventory { get; }
        public short Slot { get; }

        public AbstractModifyInventoryOperation(
            ItemInventoryType inventory,
            short slot)
        {
            Inventory = inventory;
            Slot = slot;
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) Inventory);
            packet.Encode<short>(Slot);

            EncodeData(packet);
        }

        protected virtual void EncodeData(IPacket packet)
        {
        }
    }
}