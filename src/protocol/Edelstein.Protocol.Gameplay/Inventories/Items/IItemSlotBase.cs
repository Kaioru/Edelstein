namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotBase : IItemSlot
{
    DateTime? DateExpire { get; set; }
}
