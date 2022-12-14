namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotBase : IItemSlot
{
    long? CashItemSN { get; set; }
    DateTime? DateExpire { get; set; }
}
