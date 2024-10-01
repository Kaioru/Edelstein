using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemInventory
{
    public ItemSlotBase? this[short slot] => Items.TryGetValue(slot, out var item)
        ? item
        : null;
    
    public short SlotMax { get; set; } = 24;
    public Dictionary<short, ItemSlotBase> Items { get; set; } = new();
}
