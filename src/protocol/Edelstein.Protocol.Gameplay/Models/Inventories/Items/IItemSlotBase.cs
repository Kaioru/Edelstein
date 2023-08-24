namespace Edelstein.Protocol.Gameplay.Models.Inventories.Items;

public interface IItemSlotBase : IItemSlot
{
    DateTime? DateExpire { get; set; }
}
