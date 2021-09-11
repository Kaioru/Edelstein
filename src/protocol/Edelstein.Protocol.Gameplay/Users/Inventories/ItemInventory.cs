using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Users.Inventories
{
    public record ItemInventory
    {
        public short SlotMax { get; set; }
        public IDictionary<short, AbstractItemSlot> Items { get; set; }

        public AbstractItemSlot this[in short slot] => Items[slot];

        public ItemInventory()
        {
        }

        public ItemInventory(short slotMax)
        {
            SlotMax = slotMax;
            Items = new Dictionary<short, AbstractItemSlot>();
        }
    }
}
