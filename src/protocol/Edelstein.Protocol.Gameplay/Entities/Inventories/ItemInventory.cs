using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemInventory
{
    public short SlotMax { get; set; } = 24;
    public IDictionary<short, ItemSlotBase> Items { get; } = new Dictionary<short, ItemSlotBase>();
}
