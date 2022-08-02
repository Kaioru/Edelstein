using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public class ModifyInventoryContext : AbstractModifyInventory, IModifyInventoryContext
{
    private readonly IItemInventory _inventory;
    private readonly Queue<AbstractModifyInventoryOperation> _operations;

    public ModifyInventoryContext(IItemInventory inventory)
    {
        _inventory = inventory;
        _operations = new Queue<AbstractModifyInventoryOperation>();
    }

    public override IEnumerable<AbstractModifyInventoryOperation> Operations => _operations.AsEnumerable();

    public IItemSlot? this[short slot] => _inventory.Items.ContainsKey(slot)
        ? _inventory.Items[slot]
        : null;

    public void SetSlot(short slot, IItemSlot item) => throw new NotImplementedException();

    public void RemoveSlot(short slot) => throw new NotImplementedException();
    public void RemoveSlot(short slot, short count) => throw new NotImplementedException();

    public void MoveSlot(short from, short to) => throw new NotImplementedException();

    public void UpdateSlot(short slot) => throw new NotImplementedException();

    public void SetSlot(short slot, int templateID) => throw new NotImplementedException();
    public void SetSlot(short slot, int templateID, short count) => throw new NotImplementedException();
    public void SetSlot(short slot, IItemTemplate template) => throw new NotImplementedException();
    public void SetSlot(short slot, IItemTemplate template, short count) => throw new NotImplementedException();
}
