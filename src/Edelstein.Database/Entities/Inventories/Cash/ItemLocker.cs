using System.Collections.Generic;

namespace Edelstein.Database.Entities.Inventories.Cash
{
    public class ItemLocker
    {
        public ICollection<ItemLockerSlot> Items { get; set; }

        public ItemLocker()
        {
            Items = new List<ItemLockerSlot>();
        }
    }
}