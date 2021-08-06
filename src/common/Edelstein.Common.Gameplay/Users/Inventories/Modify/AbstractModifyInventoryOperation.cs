using System;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify
{
    public abstract class AbstractModifyInventoryOperation : IModifyInventoryOperation
    {
        public abstract ModifyInventoryOperations Operation { get; }

        public ItemInventoryType Type { get; init; }
        public short Slot { get; init; }
        public DateTime Timestamp { get; init; }

        public AbstractModifyInventoryOperation()
            => Timestamp = DateTime.UtcNow;

        public void WriteBase(IPacketWriter writer)
        {
            writer.WriteByte((byte)Operation);
            writer.WriteByte((byte)Type);
            writer.WriteShort(Slot);

            WriteData(writer);
        }

        public virtual void WriteData(IPacketWriter writer) { }
    }
}
