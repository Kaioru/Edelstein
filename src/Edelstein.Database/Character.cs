using System.Collections.Generic;
using Edelstein.Database.Inventories;
using Marten.Schema;

namespace Edelstein.Database
{
    public class Character
    {
        public int ID { get; set; }
        [ForeignKey(typeof(AccountData))] public int AccountDataID { get; set; }

        [UniqueIndex(IndexType = UniqueIndexType.Computed)]
        public string Name { get; set; }

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

        public IDictionary<ItemInventoryType, ItemInventory> Inventories { get; set; }

        public Character()
        {
            Inventories = new Dictionary<ItemInventoryType, ItemInventory>
            {
                [ItemInventoryType.Equip] = new ItemInventory(24),
                [ItemInventoryType.Consume] = new ItemInventory(24),
                [ItemInventoryType.Install] = new ItemInventory(24),
                [ItemInventoryType.Etc] = new ItemInventory(24),
                [ItemInventoryType.Cash] = new ItemInventory(24)
            };
        }
    }
}