using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IItemInventory
{
    short SlotMax { get; set; }

    IDictionary<short, IItemSlot> Items { get; }
}
