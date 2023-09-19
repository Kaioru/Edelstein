namespace Edelstein.Protocol.Gameplay.Models.Inventories.Items;

public interface IItemSlotBase : IItemSlot
{
    long? CashItemSN { get; set; }
    DateTime? DateExpire { get; set; }
}
