using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities.Inventory
{
    public class ItemLockerSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public long SN { get; set; }

        public int ItemID { get; set; }
        public int CommoditySN { get; set; }
        public short Number { get; set; }
        public string BuyCharacterName { get; set; }
        public DateTime? DateExpire { get; set; }
        public int PaybackRate;
        public int DiscountRate;
    }
}