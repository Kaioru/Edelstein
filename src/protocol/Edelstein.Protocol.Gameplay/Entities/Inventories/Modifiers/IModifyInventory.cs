using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public interface IModifyInventory<in TSlot> where TSlot : ItemSlotBase
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    short Add(TSlot item);

    void Remove(int templateID, short count = 1);

    void RemoveAll(int templateID);

    void Gather();
    void Sort();
    void Clear();

    StructuredModifyInventoryOperations ToStructured();
}

public interface IModifyInventory : IModifyInventory<ItemSlotBase>
{
    short Add(int templateID, short count = 1);
    short Add(IItemTemplate template, short count = 1);
}
