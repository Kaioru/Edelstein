using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventory<in TSlot> : IPacketWritable where TSlot : IItemSlot
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    void Add(TSlot item);

    void Remove(int templateID);
    void Remove(int templateID, short count);
    void Remove(IItemTemplate template);
    void Remove(IItemTemplate template, short count);

    void RemoveAll(int templateID);
    void RemoveAll(IItemTemplate template);

    void Gather();
    void Sort();
}

public interface IModifyInventory : IModifyInventory<IItemSlot>
{
    void Add(int templateID);
    void Add(int templateID, short count);
    void Add(IItemTemplate template);
    void Add(IItemTemplate template, short count);
}
