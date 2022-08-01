using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemInventory : IItemInventory
{
    public short SlotMax { get; set; }
    public IDictionary<short, IItemSlot> Items { get; }
}
