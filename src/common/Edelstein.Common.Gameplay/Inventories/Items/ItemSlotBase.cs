namespace Edelstein.Common.Gameplay.Inventories.Items;

public record ItemSlotBase : ItemSlot
{
    public long? CashItemSN { get; set; }
    public DateTime? DateExpire { get; set; }
}
