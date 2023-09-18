namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IItemLocker
{
    short SlotMax { get; set; }
    ICollection<IItemLockerSlot> Items { get; }
}
