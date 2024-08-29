using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemTrunk
{
    public int Money { get; set; } = 0;
    public short SlotMax { get; set; } = 4;
    public ICollection<ItemSlotBase> Items { get; } = new List<ItemSlotBase>();
}
