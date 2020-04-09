using System.Collections.Generic;
using Edelstein.Entities.Inventories.Items;

namespace Edelstein.Entities.Inventories
{
    public class ItemInventory
    {
        public short SlotMax { get; set; }
        public IDictionary<short, ItemSlot> Items { get; set; }
        public ItemSlot this[in short slot] => Items[slot];

        public ItemInventory()
        {
        }

        public ItemInventory(short slotMax)
        {
            SlotMax = slotMax;
            Items = new Dictionary<short, ItemSlot>();
        }
    }
}