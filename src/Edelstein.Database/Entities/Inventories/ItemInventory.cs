using System.Collections.Generic;
using Edelstein.Database.Entities.Inventories.Items;

namespace Edelstein.Database.Entities.Inventories
{
    public class ItemInventory
    {
        public short SlotMax { get; set; }
        public IDictionary<short, ItemSlot> Items { get; set; }

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