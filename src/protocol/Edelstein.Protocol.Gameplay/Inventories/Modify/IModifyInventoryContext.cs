using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventoryContext<TSlot> : IModifyInventory<TSlot> where TSlot : IItemSlot
{
    TSlot? this[short slot] { get; }

    void SetSlot(short slot, TSlot item);

    void RemoveSlot(short slot);

    void MoveSlot(short from, short to);

    void UpdateSlot(short slot);
}

public interface IModifyInventoryContext : IModifyInventoryContext<IItemSlot>, IModifyInventory
{
    void SetSlot(short slot, int templateID);
    void SetSlot(short slot, int templateID, short count);
    void SetSlot(short slot, IItemTemplate template);
    void SetSlot(short slot, IItemTemplate template, short count);

    IItemSlot? TakeSlot(short slot);
    IItemSlot? TakeSlot(short slot, short count);
}
