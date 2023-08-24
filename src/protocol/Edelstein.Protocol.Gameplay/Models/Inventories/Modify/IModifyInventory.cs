using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

public interface IModifyInventory<TSlot> : IPacketWritable where TSlot : IItemSlot
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    bool HasItem(int templateID);
    bool HasItem(int templateID, short count);
    bool HasItem(IItemTemplate template);
    bool HasItem(IItemTemplate template, short count);

    bool HasSlotFor(int templateID);
    bool HasSlotFor(int templateID, short count);
    
    bool HasSlotFor(ICollection<Tuple<int, short>> templates);
    
    bool HasSlotFor(IItemTemplate template);
    bool HasSlotFor(IItemTemplate template, short count);
    bool HasSlotFor(ICollection<Tuple<IItemTemplate, short>> templates);

    bool HasSlotFor(IItemSlot item);
    bool HasSlotFor(ICollection<IItemSlot> items);

    void Add(TSlot item);

    void Remove(int templateID);
    void Remove(int templateID, short count);
    void Remove(IItemTemplate template);
    void Remove(IItemTemplate template, short count);

    void RemoveAll(int templateID);
    void RemoveAll(IItemTemplate template);

    void Gather();
    void Sort();
    void Clear();
}

public interface IModifyInventory : IModifyInventory<IItemSlot>
{
    void Add(int templateID);
    void Add(int templateID, short count);
    void Add(IItemTemplate template);
    void Add(IItemTemplate template, short count);
}
