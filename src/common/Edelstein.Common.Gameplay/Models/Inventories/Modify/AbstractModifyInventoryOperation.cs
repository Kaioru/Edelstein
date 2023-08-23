﻿using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify;

public abstract class AbstractModifyInventoryOperation : IPacketWritable
{

    protected AbstractModifyInventoryOperation(
        ModifyInventoryOperationType type,
        ItemInventoryType inventory,
        short slot
    )
    {
        Type = type;
        Inventory = inventory;
        Slot = slot;
    }
    public ModifyInventoryOperationType Type { get; }
    public ItemInventoryType Inventory { get; }
    public short Slot { get; }

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
