using System.Collections.Generic;
using Edelstein.Database.Inventories.Items;

namespace Edelstein.Database.Inventories
{
    public class ItemInventory
    {
        public short SlotMax { get; set; }
        public IDictionary<short, ItemSlot> Items { get; set; }

        public ItemInventory(short slotMax)
        {
            SlotMax = slotMax;
            Items = new Dictionary<short, ItemSlot>();
        }
    }
}