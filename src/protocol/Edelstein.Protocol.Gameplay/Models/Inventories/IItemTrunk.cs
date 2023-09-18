using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IItemTrunk
{
    int Money { get; set; }
    short SlotMax { get; set; }
    ICollection<IItemSlot> Items { get; }
}
