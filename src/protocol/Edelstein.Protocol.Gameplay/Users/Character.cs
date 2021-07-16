using System;
using System.Collections.Generic;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users.Inventories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public class Character : IDataDocument
    {
        public int ID { get; init; }
        public int AccountWorldID { get; init; }

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

        //public FunctionKey[] FunctionKeys { get; set; }
        //public int[] QuickSlotKeys { get; set; }

        public IDictionary<ItemInventoryType, ItemInventory> Inventories { get; set; }
        //public IDictionary<int, SkillRecord> SkillRecord { get; set; }
        //public IDictionary<short, string> QuestRecord { get; set; }
        //public IDictionary<short, string> QuestRecordEx { get; set; }
        //public IDictionary<short, DateTime> QuestComplete { get; set; }

        public int[] WishList { get; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Character()
        {
            Level = 1;
            STR = 4;
            DEX = 4;
            INT = 4;
            LUK = 4;
            HP = 50;
            MaxHP = 50;
            MP = 50;
            MaxMP = 50;

            Pets = new long[3];
            ExtendSP = new Dictionary<byte, byte>();

            //FunctionKeys = new FunctionKey[90];
            //FunctionKeys[2] = new FunctionKey(KeyType.Menu, KeyMenu.ChatAll);
            //FunctionKeys[3] = new FunctionKey(KeyType.Menu, KeyMenu.ChatParty);
            //FunctionKeys[4] = new FunctionKey(KeyType.Menu, KeyMenu.ChatFriend);
            //FunctionKeys[5] = new FunctionKey(KeyType.Menu, KeyMenu.ChatGuild);
            //FunctionKeys[6] = new FunctionKey(KeyType.Menu, KeyMenu.ChatAlliance);
            //FunctionKeys[7] = new FunctionKey(KeyType.Menu, KeyMenu.ChatCouple);
            //FunctionKeys[8] = new FunctionKey(KeyType.Menu, KeyMenu.ChatExpedition);
            //FunctionKeys[16] = new FunctionKey(KeyType.Menu, KeyMenu.Quest);
            //FunctionKeys[17] = new FunctionKey(KeyType.Menu, KeyMenu.WorldMap);
            //FunctionKeys[18] = new FunctionKey(KeyType.Menu, KeyMenu.Equip);
            //FunctionKeys[19] = new FunctionKey(KeyType.Menu, KeyMenu.Friend);
            //FunctionKeys[20] = new FunctionKey(KeyType.Menu, KeyMenu.Expedition);
            //FunctionKeys[23] = new FunctionKey(KeyType.Menu, KeyMenu.Item);
            //FunctionKeys[24] = new FunctionKey(KeyType.Menu, KeyMenu.PartySearch);
            //FunctionKeys[25] = new FunctionKey(KeyType.Menu, KeyMenu.Party);
            //FunctionKeys[26] = new FunctionKey(KeyType.Menu, KeyMenu.Shortcut);
            //FunctionKeys[27] = new FunctionKey(KeyType.Menu, KeyMenu.QuickSlot);
            //FunctionKeys[29] = new FunctionKey(KeyType.BasicAction, KeyMenu.Attack);
            //FunctionKeys[31] = new FunctionKey(KeyType.Menu, KeyMenu.Stat);
            //FunctionKeys[33] = new FunctionKey(KeyType.Menu, KeyMenu.Family);
            //FunctionKeys[34] = new FunctionKey(KeyType.Menu, KeyMenu.Guild);
            //FunctionKeys[35] = new FunctionKey(KeyType.Menu, KeyMenu.ChatWhisper);
            //FunctionKeys[37] = new FunctionKey(KeyType.Menu, KeyMenu.Skill);
            //FunctionKeys[38] = new FunctionKey(KeyType.Menu, KeyMenu.QuestAlarm);
            //FunctionKeys[39] = new FunctionKey(KeyType.Menu, KeyMenu.MedalQuest);
            //FunctionKeys[40] = new FunctionKey(KeyType.Menu, KeyMenu.ChatType);
            //FunctionKeys[41] = new FunctionKey(KeyType.Menu, KeyMenu.CashShop);
            //FunctionKeys[43] = new FunctionKey(KeyType.Menu, KeyMenu.KeyConfig);
            //FunctionKeys[44] = new FunctionKey(KeyType.BasicAction, KeyMenu.Pickup);
            //FunctionKeys[45] = new FunctionKey(KeyType.BasicAction, KeyMenu.Sit);
            //FunctionKeys[46] = new FunctionKey(KeyType.Menu, KeyMenu.Messenger);
            //FunctionKeys[50] = new FunctionKey(KeyType.Menu, KeyMenu.MiniMap);
            //FunctionKeys[56] = new FunctionKey(KeyType.BasicAction, KeyMenu.Jump);
            //FunctionKeys[57] = new FunctionKey(KeyType.BasicAction, KeyMenu.NpcTalk);
            //FunctionKeys[59] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion0);
            //FunctionKeys[60] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion1);
            //FunctionKeys[61] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion2);
            //FunctionKeys[62] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion3);
            //FunctionKeys[63] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion4);
            //FunctionKeys[64] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion5);
            //FunctionKeys[65] = new FunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion6);

            //QuickSlotKeys = new int[8];
            //QuickSlotKeys[0] = 42;
            //QuickSlotKeys[1] = 82;
            //QuickSlotKeys[2] = 71;
            //QuickSlotKeys[3] = 73;
            //QuickSlotKeys[4] = 29;
            //QuickSlotKeys[5] = 83;
            //QuickSlotKeys[6] = 79;
            //QuickSlotKeys[7] = 81;

            Inventories = new Dictionary<ItemInventoryType, ItemInventory>
            {
                [ItemInventoryType.Equip] = new ItemInventory(24),
                [ItemInventoryType.Consume] = new ItemInventory(24),
                [ItemInventoryType.Install] = new ItemInventory(24),
                [ItemInventoryType.Etc] = new ItemInventory(24),
                [ItemInventoryType.Cash] = new ItemInventory(24)
            };
            //SkillRecord = new Dictionary<int, SkillRecord>();
            //QuestRecord = new Dictionary<short, string>();
            //QuestRecordEx = new Dictionary<short, string>();
            //QuestComplete = new Dictionary<short, DateTime>();

            WishList = new int[10];
        }
    }
}