using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Modify;

namespace Edelstein.Common.Gameplay.Inventories.Modify;

public class ModifyInventoryGroupContext : AbstractModifyInventory, IModifyInventoryGroupContext
{
    private readonly Dictionary<ItemInventoryType, ModifyInventoryContext> _contexts;

    public ModifyInventoryGroupContext(IDictionary<ItemInventoryType, IItemInventory> inventories) =>
        _contexts = inventories.ToDictionary(
            kv => kv.Key,
            kv => new ModifyInventoryContext(kv.Value)
        );

    public override IEnumerable<AbstractModifyInventoryOperation> Operations =>
        _contexts.Values.SelectMany(c => c.Operations);

    public IModifyInventoryContext? this[ItemInventoryType type] =>
        _contexts.GetValueOrDefault(type);
}
