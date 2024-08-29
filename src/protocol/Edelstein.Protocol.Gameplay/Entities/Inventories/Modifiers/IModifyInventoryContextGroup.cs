using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public interface IModifyInventoryContextGroup<in TSlot, out TContext> : IModifyInventory<TSlot>
    where TSlot : ItemSlotBase
    where TContext : IModifyInventoryContext<TSlot>
{
    TContext? this[ItemInventoryType type] { get; }
}

public interface IModifyInventoryContextGroup :
    IModifyInventoryContextGroup<ItemSlotBase, IModifyInventoryContext>,
    IModifyInventory
{
    void SetEquipped(BodyPart part, int templateID, short count = 1);
    void SetEquipped(BodyPart part, IItemTemplate template, short count = 1);
}
