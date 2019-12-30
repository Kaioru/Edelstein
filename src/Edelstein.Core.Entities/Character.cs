using System;
using System.Collections.Generic;
using Edelstein.Database;
using Edelstein.Entities.Inventories;

namespace Edelstein.Entities
{
    public class Character : IDataEntity
    {
        public int ID { get; set; }

        public int AccountWorldID { get; set; }

        public string Name { get; set; }
        public byte Gender { get; set; }
        public byte Skin { get; set; }
        public int Face { get; set; }
        public int Hair { get; set; }

        public long[] Pets { get; set; }

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
        public IDictionary<byte, byte> ExtendSP { get; set; }

        public int EXP { get; set; }
        public short POP { get; set; }

        public int Money { get; set; }
        public int TempEXP { get; set; }

        public int FieldID { get; set; }
        public byte FieldPortal { get; set; }

        public int PlayTime { get; set; }

        public short SubJob { get; set; }

        // Function Keys
        // Quickslot Keys

        public IDictionary<ItemInventoryType, ItemInventory> Inventories { get; set; }

        public Character()
        {
            Pets = new long[3];
            ExtendSP = new Dictionary<byte, byte>();

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