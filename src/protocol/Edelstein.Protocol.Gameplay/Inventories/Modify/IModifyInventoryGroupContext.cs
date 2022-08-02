using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventoryGroupContext<TSlot, TContext> : IModifyInventory<TSlot>
    where TSlot : IItemSlot
    where TContext : IModifyInventoryContext<TSlot>
{
    TContext? this[ItemInventoryType type] { get; }
}

public interface IModifyInventoryGroupContext : IModifyInventoryGroupContext<IItemSlot, IModifyInventoryContext>
{
}
