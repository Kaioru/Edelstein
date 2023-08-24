using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public record ItemInventory : IItemInventory
{
    public short SlotMax { get; set; } = 24;

    public IDictionary<short, IItemSlot> Items { get; set; } = new Dictionary<short, IItemSlot>();
}
