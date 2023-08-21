using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public record ItemTrunk : IItemTrunk
{
    public short SlotMax { get; set; } = 4;
    public int Money { get; set; }

    public IDictionary<short, IItemSlot> Items { get; } = new Dictionary<short, IItemSlot>();
}
