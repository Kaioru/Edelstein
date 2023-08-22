using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

public interface IModifyInventoryContext<TSlot> : IModifyInventory<TSlot> where TSlot : IItemSlot
{
    TSlot? this[short slot] { get; }
    IReadOnlyDictionary<short, TSlot> Items { get; }

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

    void UpdateQuantitySlot(short slot, short count);
    void UpdateEXPSlot(short slot, int exp);
}
