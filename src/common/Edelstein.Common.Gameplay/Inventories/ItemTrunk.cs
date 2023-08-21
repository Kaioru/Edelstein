using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemTrunk : IItemTrunk
{
    public short SlotMax { get; set; } = 4;
    public int Money { get; set; }

    public IDictionary<short, IItemSlot> Items { get; } = new Dictionary<short, IItemSlot>();
}
