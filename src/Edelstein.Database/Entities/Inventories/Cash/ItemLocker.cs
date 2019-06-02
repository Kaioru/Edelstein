using System.Collections.Generic;

namespace Edelstein.Database.Entities.Inventories.Cash
{
    public class ItemLocker
    {
        public short SlotMax { get; set; }
        public IList<ItemLockerSlot> Items { get; set; }

        public ItemLocker()
        {
        }

        public ItemLocker(short slotMax)
        {
            SlotMax = slotMax;
            Items = new List<ItemLockerSlot>();
        }
    }
}