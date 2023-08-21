using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemInventory : IItemInventory
{
    public short SlotMax { get; set; } = 24;
    
    public IDictionary<short, IItemSlot> Items { get; set; } = new Dictionary<short, IItemSlot>();
}
