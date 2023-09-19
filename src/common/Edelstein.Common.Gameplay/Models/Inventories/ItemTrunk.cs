using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public record ItemTrunk : IItemTrunk
{
    public int Money { get; set; }
    public short SlotMax { get; set; } = 4;
    public ICollection<IItemSlot> Items { get; } = new List<IItemSlot>();
}
