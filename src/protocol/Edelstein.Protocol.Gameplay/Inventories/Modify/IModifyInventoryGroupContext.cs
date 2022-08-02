namespace Edelstein.Protocol.Gameplay.Inventories.Modify;

public interface IModifyInventoryGroupContext
{
    IModifyInventoryContext? this[ItemInventoryType type] { get; }
}
