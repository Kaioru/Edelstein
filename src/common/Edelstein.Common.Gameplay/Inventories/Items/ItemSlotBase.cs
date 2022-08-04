using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories.Items;

public record ItemSlotBase : ItemSlot, IItemSlot
{
    public long? CashItemSN { get; set; }
    public DateTime? DateExpire { get; set; }
}
