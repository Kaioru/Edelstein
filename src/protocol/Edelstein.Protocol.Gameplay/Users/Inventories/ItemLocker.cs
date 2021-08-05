using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Users.Inventories
{
    public class ItemLocker
    {
        public short SlotMax { get; set; }
        public IList<ItemLockerSlot> Items { get; set; }

        public ItemLockerSlot this[in short slot] => Items[slot];

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
