using System.Collections.Immutable;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public abstract class AbstractModifyInventory : IModifyInventory
{
    public abstract IEnumerable<AbstractModifyInventoryOperation> Operations { get; }

    public bool IsUpdated => Operations.Any();
    public bool IsUpdatedAvatar => Operations.Any(o => o.Inventory == ItemInventoryType.Equip && o.Slot < 0);

    public abstract void Add(IItemSlot item);

    public abstract void Remove(int templateID);
    public abstract void Remove(int templateID, short count);
    public abstract void Remove(IItemTemplate template);
    public abstract void Remove(IItemTemplate template, short count);

    public abstract void RemoveAll(int templateID);
    public abstract void RemoveAll(IItemTemplate template);

    public abstract void Gather();
    public abstract void Sort();

    public abstract void Add(int templateID);
    public abstract void Add(int templateID, short count);
    public abstract void Add(IItemTemplate template);
    public abstract void Add(IItemTemplate template, short count);

    public void WriteTo(IPacketWriter writer)
    {
        var operations = Operations.ToImmutableList();

        writer.WriteByte((byte)operations.Count);
        foreach (var operation in operations)
            writer.Write(operation);
    }
}
