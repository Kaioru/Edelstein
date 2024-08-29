using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public interface IModifyInventoryContext<TSlot> : IModifyInventory<TSlot> where TSlot : ItemSlotBase
{
    TSlot? this[short slot] { get; }
    
    void SetSlot(short slot, TSlot item);

    void RemoveSlot(short slot);

    void MoveSlot(short from, short to);

    void UpdateSlot(short slot);
}

public interface IModifyInventoryContext : IModifyInventoryContext<ItemSlotBase>, IModifyInventory
{
    void SetSlot(short slot, int templateID, short count = 1);
    void SetSlot(short slot, IItemTemplate template, short count = 1);

    ItemSlotBase? TakeSlot(short slot, short count = 1);

    void UpdateNumberSlot(short slot, short count);
    void UpdateEXPSlot(short slot, int exp);
}
