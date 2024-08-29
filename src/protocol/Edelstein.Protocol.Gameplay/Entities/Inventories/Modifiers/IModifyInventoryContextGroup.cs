using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public interface IModifyInventoryContextGroup<TSlot, out TContext> : IModifyInventory<TSlot>
    where TSlot : ItemSlotBase
    where TContext : IModifyInventoryContext<TSlot>
{
    TContext? this[ItemInventoryType type] { get; }
}

public interface IModifyInventoryContextGroup :
    IModifyInventoryContextGroup<ItemSlotBase, IModifyInventoryContext>,
    IModifyInventory
{
    bool HasEquipped(int templateID);
    bool HasEquipped(IItemTemplate template);

    void SetEquipped(BodyPart part, int templateID);
    void SetEquipped(BodyPart part, int templateID, short count);
    void SetEquipped(BodyPart part, IItemTemplate template);
    void SetEquipped(BodyPart part, IItemTemplate template, short count);
}
