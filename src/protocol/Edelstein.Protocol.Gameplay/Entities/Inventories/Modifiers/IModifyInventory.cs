using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public interface IModifyInventory<in TSlot> where TSlot : ItemSlotBase
{
    bool IsUpdated { get; }
    bool IsUpdatedAvatar { get; }

    short Add(TSlot item);

    Task Remove(int templateID);
    Task Remove(int templateID, short count);

    Task RemoveAll(int templateID);

    void Gather();
    void Sort();
    void Clear();

    StructuredModifyInventoryOperations ToStructured();
}

public interface IModifyInventory : IModifyInventory<ItemSlotBase>
{
    Task<short> Add(int templateID);
    Task<short> Add(int templateID, short count);
    short Add(IItemTemplate template);
    short Add(IItemTemplate template, short count);
}
