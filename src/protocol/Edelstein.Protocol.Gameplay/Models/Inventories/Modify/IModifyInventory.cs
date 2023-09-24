using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

public interface IModifyInventory<in TSlot> : IPacketWritable where TSlot : IItemSlot
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    short Add(TSlot item);

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
    short Add(int templateID);
    short Add(int templateID, short count);
    short Add(IItemTemplate template);
    short Add(IItemTemplate template, short count);
}
