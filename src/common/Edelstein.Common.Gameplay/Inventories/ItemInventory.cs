using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemInventory : IItemInventory
{
    public ItemInventory(IDictionary<short, IItemSlot> items) => Items = items;

    public ItemInventory()
    {
    }

    public ItemInventory(short slotMax)
    {
        SlotMax = slotMax;
        Items = new Dictionary<short, IItemSlot>();
    }

    public short SlotMax { get; set; }
    public IDictionary<short, IItemSlot> Items { get; set; }
}
