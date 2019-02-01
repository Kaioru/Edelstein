using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Edelstein.Data.Entities.Inventory;

namespace Edelstein.Data.Entities
{
    public class AccountData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Account Account { get; set; }

        public byte WorldID { get; set; }
        public int SlotCount { get; set; }

        public ItemLocker Locker { get; set; }
        public ItemTrunk Trunk { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}