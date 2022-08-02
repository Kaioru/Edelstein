using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public abstract class AbstractModifyInventoryOperation : IPacketWritable
{
    public ModifyInventoryOperationType Type { get; init; }
    public ItemInventoryType Inventory { get; init; }
    public short Slot { get; init; }

    public void WriteTo(IPacketWriter writer)
    {
        WriteHeader(writer);
        WriteData(writer);
    }

    protected virtual void WriteHeader(IPacketWriter writer)
    {
        writer.WriteByte((byte)Type);
        writer.WriteByte((byte)Inventory);
        writer.WriteShort(Slot);
    }

    protected virtual void WriteData(IPacketWriter writer)
    {
    }
}
