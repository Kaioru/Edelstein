using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Edelstein.Data.Entities.Inventory;

namespace Edelstein.Data.Entities
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public AccountData Data { get; set; }

        [MaxLength(13)] public string Name { get; set; }

        public byte Gender { get; set; }
        public byte Skin { get; set; }
        public int Face { get; set; }
        public int Hair { get; set; }

        public byte Level { get; set; }
        public short Job { get; set; }
        public short STR { get; set; }
        public short DEX { get; set; }
        public short INT { get; set; }
        public short LUK { get; set; }

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }

        public short AP { get; set; }
        public short SP { get; set; }

        public int EXP { get; set; }
        public short POP { get; set; }

        public int Money { get; set; }
        public int TempEXP { get; set; }

        public int FieldID { get; set; }
        public byte FieldPortal { get; set; }

        public int PlayTime { get; set; }

        public short SubJob { get; set; }

        public ICollection<ItemInventory> Inventories { get; set; }
        public ICollection<WishList> WishList { get; set; }

        public ItemInventory GetInventory(ItemInventoryType type)
            => Inventories.Single(i => i.Type == type);
    }
}