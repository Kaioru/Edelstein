using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities.Inventory
{
    public class ItemTrunk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public byte SlotMax { get; set; }
        public int Money { get; set; }

        public ICollection<ItemSlot> Items { get; set; }

        public ItemTrunk(byte slotMax)
        {
            SlotMax = slotMax;
            Items = new List<ItemSlot>();
        }
    }
}