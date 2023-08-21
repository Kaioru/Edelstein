namespace Edelstein.Common.Gameplay.Models.Inventories.Items;

public record ItemSlotBase : ItemSlot
{
    public DateTime? DateExpire { get; set; }
}
