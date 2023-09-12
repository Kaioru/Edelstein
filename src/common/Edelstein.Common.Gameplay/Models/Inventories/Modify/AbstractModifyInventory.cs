using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify;

public abstract class AbstractModifyInventory : IModifyInventory
{
    public abstract IEnumerable<AbstractModifyInventoryOperation> Operations { get; }

    public bool IsUpdated => Operations.Any();
    public bool IsUpdatedAvatar => Operations.Any(o => o is 
        { Inventory: ItemInventoryType.Equip, Slot: < 0 } or 
        MoveModifyInventoryOperation { ToSlot: < 0 });

    public abstract bool HasItem(int templateID);
    public abstract bool HasItem(int templateID, short count);
    public abstract bool HasItem(IItemTemplate template);
    public abstract bool HasItem(IItemTemplate template, short count);
    
    public abstract bool HasSlotFor(int templateID);
    public abstract bool HasSlotFor(int templateID, short count);
    public abstract bool HasSlotFor(ICollection<Tuple<int, short>> templates);
    
    public abstract bool HasSlotFor(IItemTemplate template);
    public abstract bool HasSlotFor(IItemTemplate template, short count);
    public abstract bool HasSlotFor(ICollection<Tuple<IItemTemplate, short>> templates);
        
    public abstract bool HasSlotFor(IItemSlot item);
    public abstract bool HasSlotFor(ICollection<IItemSlot> items);

    public abstract void Add(IItemSlot item);

    public abstract void Remove(int templateID);
    public abstract void Remove(int templateID, short count);
    public abstract void Remove(IItemTemplate template);
    public abstract void Remove(IItemTemplate template, short count);

    public abstract void RemoveAll(int templateID);
    public abstract void RemoveAll(IItemTemplate template);

    public abstract void Gather();
    public abstract void Sort();
    public abstract void Clear();

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
