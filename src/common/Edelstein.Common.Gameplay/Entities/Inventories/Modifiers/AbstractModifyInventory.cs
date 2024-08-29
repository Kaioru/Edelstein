using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers.Operations;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;

public abstract class AbstractModifyInventory : IModifyInventory
{
    public abstract IEnumerable<StructuredModifyInventoryOperation> Operations { get; }

    public bool IsUpdated => Operations.Any();
    public bool IsUpdatedAvatar => Operations
        .Any(o => o is
            { Info: { Inventory: ItemInventoryType.Equip, Slot: < 0 } } or
            { Info: StructuredModifyInventoryOperationInfoMove { ToSlot: < 0 } }
        );

    public abstract short Add(ItemSlotBase item);
    public abstract short Add(int templateID, short count = 1);
    public abstract short Add(IItemTemplate template, short count = 1);
    
    public abstract void Remove(int templateID, short count = 1);
    public abstract void RemoveAll(int templateID);
    
    public abstract void Gather();
    public abstract void Sort();
    public abstract void Clear();

    public StructuredModifyInventoryOperations ToStructured()
        => new()
        {
            Operations = Operations.ToList()
        };
}
