using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventory : IPacketWritable
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    void Add(int templateID);
    void Add(int templateID, short count);
    void Add(IItemSlot item);

    IItemSlotBundle? Take(int templateID);
    IItemSlotBundle? Take(int templateID, short count);
    IItemSlotBundle? Take(IItemSlotBundle bundle);
    IItemSlotBundle? Take(IItemSlotBundle bundle, short count);

    void Remove(int templateID);
    void Remove(int templateID, short count);
    void Remove(IItemSlot item);
    void Remove(IItemSlot item, short count);

    void RemoveAll(int templateID);
    void RemoveAll(IItemSlot item);

    void Update(IItemSlot item);

    void Gather();
    void Sort();
}
