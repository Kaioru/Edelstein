using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public class ModifyInventoryContext<TSlot> : AbstractModifyInventory, IModifyInventoryContext where TSlot : IItemSlot
{
    private readonly IItemInventory<TSlot> _inventory;

    public ModifyInventoryContext(IItemInventory<TSlot> inventory)
    {
        _inventory = inventory;
        Operations = new List<AbstractModifyInventoryOperation>();
    }

    public override ICollection<AbstractModifyInventoryOperation> Operations { get; }

    public IItemSlot? this[short slot] => _inventory.Items.ContainsKey(slot)
        ? _inventory.Items[slot]
        : null;

    public void SetSlot(short slot, int templateID) => throw new NotImplementedException();
    public void SetSlot(short slot, int templateID, short count) => throw new NotImplementedException();
    public void SetSlot(short slot, IItemSlot item) => throw new NotImplementedException();

    public IItemSlotBundle? TakeSlot(short slot) => throw new NotImplementedException();
    public IItemSlotBundle? TakeSlot(short slot, short count) => throw new NotImplementedException();

    public void RemoveSlot(short slot) => throw new NotImplementedException();
    public void RemoveSlot(short slot, short count) => throw new NotImplementedException();

    public void MoveSlot(short from, short to) => throw new NotImplementedException();

    public void UpdateSlot(short slot) => throw new NotImplementedException();
}
