using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Modify;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public class ModifyInventoryGroupContext<TSlot> : AbstractModifyInventory, IModifyInventoryGroupContext
    where TSlot : IItemSlot
{
    private readonly IDictionary<ItemInventoryType, ModifyInventoryContext<TSlot>> _inventories;

    public ModifyInventoryGroupContext(IDictionary<ItemInventoryType, IItemInventory<TSlot>> inventories) =>
        _inventories = inventories.ToDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext<TSlot>(kv.Value)
        );

    public override ICollection<AbstractModifyInventoryOperation> Operations =>
        _inventories.Values.SelectMany(i => i.Operations).ToList();

    public IModifyInventoryContext? this[ItemInventoryType type] => _inventories.ContainsKey(type)
        ? _inventories[type]
        : null;
}
