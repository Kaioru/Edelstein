using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventoryContext
{
    IItemSlot? this[short slot] { get; }

    void SetSlot(short slot, int templateID);
    void SetSlot(short slot, int templateID, short count);
    void SetSlot(short slot, IItemSlot item);

    IItemSlotBundle? TakeSlot(short slot);
    IItemSlotBundle? TakeSlot(short slot, short count);

    void RemoveSlot(short slot);
    void RemoveSlot(short slot, short count);

    void MoveSlot(short from, short to);

    void UpdateSlot(short slot);
}
