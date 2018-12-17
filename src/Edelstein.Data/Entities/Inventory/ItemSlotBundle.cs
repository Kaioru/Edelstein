using System.ComponentModel.DataAnnotations;

namespace Edelstein.Data.Entities.Inventory
{
    public class ItemSlotBundle : ItemSlot
    {
        public short Number { get; set; }
        public short MaxNumber { get; set; }
        public short Attribute { get; set; }

        [MaxLength(13)] public string Title { get; set; }
    }
}