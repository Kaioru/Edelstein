using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemInventory
{
    public short SlotMax { get; set; } = 24;
    public Dictionary<short, ItemSlotBase> Items { get; set; } = new();
}
