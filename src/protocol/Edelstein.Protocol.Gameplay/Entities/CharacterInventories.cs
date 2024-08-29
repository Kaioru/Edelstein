using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities.Inventories;

namespace Edelstein.Protocol.Gameplay.Entities;

public record CharacterInventories
{
    public ItemInventory? this[ItemInventoryType type] => new Dictionary<ItemInventoryType, ItemInventory>
    {
        { ItemInventoryType.Equip, Equip },
        { ItemInventoryType.Consume, Consume },
        { ItemInventoryType.Install, Install },
        { ItemInventoryType.Etc, Etc },
        { ItemInventoryType.Cash, Cash }
    }.TryGetValue(type, out var result) ? result : null;

    public ItemInventory Equip { get; set; } = new();
    public ItemInventory Consume { get; set; } = new();
    public ItemInventory Install { get; set; } = new();
    public ItemInventory Etc { get; set; } = new();
    public ItemInventory Cash { get; set; } = new();
}
