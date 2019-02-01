using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities.Inventory
{
    public class ItemLocker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public short SlotMax { get; set; }
        public ICollection<ItemLockerSlot> Items { get; set; }

        public ItemLocker(short slotMax)
        {
            SlotMax = slotMax;
            Items = new List<ItemLockerSlot>();
        }
    }
}