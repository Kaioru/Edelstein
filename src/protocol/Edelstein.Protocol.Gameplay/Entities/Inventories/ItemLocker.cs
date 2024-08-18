using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemLocker
{
    public short SlotMax { get; set; } = 999;
    public ICollection<ItemLockerSlot> Items { get; } = new List<ItemLockerSlot>();
}
