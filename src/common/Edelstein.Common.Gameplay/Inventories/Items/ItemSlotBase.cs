namespace Edelstein.Common.Gameplay.Inventories.Items;

public record ItemSlotBase : ItemSlot
{
    public DateTime? DateExpire { get; set; }
}
