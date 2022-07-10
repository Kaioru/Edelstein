using System;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify
{
    public abstract class AbstractModifyInventoryOperation : IModifyInventoryOperation, IPacketWritable
    {
        public abstract ModifyInventoryOperations Operation { get; }

        public ItemInventoryType Type { get; init; }
        public short Slot { get; init; }
        public DateTime Timestamp { get; init; }

        public AbstractModifyInventoryOperation()
            => Timestamp = DateTime.UtcNow;

        public void WriteToPacket(IPacketWriter writer)
        {
            WriteBase(writer);
            WriteData(writer);
        }

        protected void WriteBase(IPacketWriter writer)
        {
            writer.WriteByte((byte)Operation);
            writer.WriteByte((byte)Type);
            writer.WriteShort(Slot);
        }

        protected virtual void WriteData(IPacketWriter writer) { }
    }
}
