using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

public interface IModifyInventory<in TSlot> : IPacketWritable where TSlot : IItemSlot
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    bool Check(int templateID);
    bool Check(int templateID, short count);
    bool Check(IItemTemplate template);
    bool Check(IItemTemplate template, short count);

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
