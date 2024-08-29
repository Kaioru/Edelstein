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
    Task SetSlot(short slot, int templateID);
    Task SetSlot(short slot, int templateID, short count);
    void SetSlot(short slot, IItemTemplate template);
    void SetSlot(short slot, IItemTemplate template, short count);

    ItemSlotBase? TakeSlot(short slot);
    ItemSlotBase? TakeSlot(short slot, short count);

    void UpdateNumberSlot(short slot, short count);
    void UpdateEXPSlot(short slot, int exp);
}
