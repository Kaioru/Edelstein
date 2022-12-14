using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventoryGroupContext<in TSlot, out TContext> : IModifyInventory<TSlot>
    where TSlot : IItemSlot
    where TContext : IModifyInventoryContext<TSlot>
{
    TContext? this[ItemInventoryType type] { get; }
}

public interface IModifyInventoryGroupContext :
    IModifyInventoryGroupContext<IItemSlot, IModifyInventoryContext>,
    IModifyInventory
{
    void SetEquipped(BodyPart part, int templateID);
    void SetEquipped(BodyPart part, int templateID, short count);
    void SetEquipped(BodyPart part, IItemTemplate template);
    void SetEquipped(BodyPart part, IItemTemplate template, short count);
}
