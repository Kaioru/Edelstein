using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterInventories
{
    IItemInventory? this[ItemInventoryType type] { get; }

    IItemInventory Equip { get; set; }
    IItemInventory Consume { get; set; }
    IItemInventory Install { get; set; }
    IItemInventory Etc { get; set; }
    IItemInventory Cash { get; set; }
}
