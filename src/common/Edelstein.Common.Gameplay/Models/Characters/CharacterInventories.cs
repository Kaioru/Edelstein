using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterInventories : ICharacterInventories
{
    public IItemInventory? this[ItemInventoryType type] => new Dictionary<ItemInventoryType, IItemInventory>
    {
        { ItemInventoryType.Equip, Equip },
        { ItemInventoryType.Consume, Consume },
        { ItemInventoryType.Install, Install },
        { ItemInventoryType.Etc, Etc },
        { ItemInventoryType.Cash, Cash }
    }.TryGetValue(type, out var result) ? result : null;

    public IItemInventory Equip { get; set; } = new ItemInventory();
    public IItemInventory Consume { get; set; } = new ItemInventory();
    public IItemInventory Install { get; set; } = new ItemInventory();
    public IItemInventory Etc { get; set; } = new ItemInventory();
    public IItemInventory Cash { get; set; } = new ItemInventory();
}
