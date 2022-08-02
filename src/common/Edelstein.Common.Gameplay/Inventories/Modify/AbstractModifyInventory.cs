using System.Collections.Immutable;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public abstract class AbstractModifyInventory : IModifyInventory
{
    public abstract IEnumerable<AbstractModifyInventoryOperation> Operations { get; }

    public bool IsUpdated => Operations.Any();
    public bool IsUpdatedAvatar => Operations.Any(o => o.Inventory == ItemInventoryType.Equip && o.Slot < 0);

    public void Add(int templateID) => throw new NotImplementedException();
    public void Add(int templateID, short count) => throw new NotImplementedException();
    public void Add(IItemSlot item) => throw new NotImplementedException();

    public IItemSlotBundle? Take(int templateID) => throw new NotImplementedException();
    public IItemSlotBundle? Take(int templateID, short count) => throw new NotImplementedException();
    public IItemSlotBundle? Take(IItemSlotBundle bundle) => throw new NotImplementedException();
    public IItemSlotBundle? Take(IItemSlotBundle bundle, short count) => throw new NotImplementedException();

    public void Remove(int templateID) => throw new NotImplementedException();
    public void Remove(int templateID, short count) => throw new NotImplementedException();
    public void Remove(IItemSlot item) => throw new NotImplementedException();
    public void Remove(IItemSlot item, short count) => throw new NotImplementedException();

    public void RemoveAll(int templateID) => throw new NotImplementedException();
    public void RemoveAll(IItemSlot item) => throw new NotImplementedException();

    public void Update(IItemSlot item) => throw new NotImplementedException();

    public void Gather() => throw new NotImplementedException();
    public void Sort() => throw new NotImplementedException();

    public void WriteTo(IPacketWriter writer)
    {
        var operations = Operations.ToImmutableList();

        writer.WriteByte((byte)operations.Count);
        foreach (var operation in operations)
            writer.Write(operation);
    }
}
