using System.Collections.Generic;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Keys;
using Edelstein.Protocol.Gameplay.Users.Skills;

namespace Edelstein.Protocol.Gameplay.Users
{
    public record Character : IDataDocument
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

        public CharacterFunctionKey[] FunctionKeys { get; set; }
        public CharacterQuickSlotKey[] QuickSlotKeys { get; set; }

        public IDictionary<ItemInventoryType, ItemInventory> Inventories { get; set; }
        public IDictionary<int, CharacterSkillRecord> SkillRecord { get; set; }
        //public IDictionary<short, string> QuestRecord { get; set; }
        //public IDictionary<short, string> QuestRecordEx { get; set; }
        //public IDictionary<short, DateTime> QuestComplete { get; set; }

        public int[] WishList { get; }

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

            FunctionKeys = new CharacterFunctionKey[90];
            FunctionKeys[2] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatAll);
            FunctionKeys[3] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatParty);
            FunctionKeys[4] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatFriend);
            FunctionKeys[5] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatGuild);
            FunctionKeys[6] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatAlliance);
            FunctionKeys[7] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatCouple);
            FunctionKeys[8] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatExpedition);
            FunctionKeys[16] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Quest);
            FunctionKeys[17] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.WorldMap);
            FunctionKeys[18] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Equip);
            FunctionKeys[19] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Friend);
            FunctionKeys[20] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Expedition);
            FunctionKeys[23] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Item);
            FunctionKeys[24] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.PartySearch);
            FunctionKeys[25] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Party);
            FunctionKeys[26] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Shortcut);
            FunctionKeys[27] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.QuickSlot);
            FunctionKeys[29] = new CharacterFunctionKey(KeyType.BasicAction, KeyMenu.Attack);
            FunctionKeys[31] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Stat);
            FunctionKeys[33] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Family);
            FunctionKeys[34] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Guild);
            FunctionKeys[35] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatWhisper);
            FunctionKeys[37] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Skill);
            FunctionKeys[38] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.QuestAlarm);
            FunctionKeys[39] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.MedalQuest);
            FunctionKeys[40] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.ChatType);
            FunctionKeys[41] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.CashShop);
            FunctionKeys[43] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.KeyConfig);
            FunctionKeys[44] = new CharacterFunctionKey(KeyType.BasicAction, KeyMenu.Pickup);
            FunctionKeys[45] = new CharacterFunctionKey(KeyType.BasicAction, KeyMenu.Sit);
            FunctionKeys[46] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.Messenger);
            FunctionKeys[50] = new CharacterFunctionKey(KeyType.Menu, KeyMenu.MiniMap);
            FunctionKeys[56] = new CharacterFunctionKey(KeyType.BasicAction, KeyMenu.Jump);
            FunctionKeys[57] = new CharacterFunctionKey(KeyType.BasicAction, KeyMenu.NpcTalk);
            FunctionKeys[59] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion0);
            FunctionKeys[60] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion1);
            FunctionKeys[61] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion2);
            FunctionKeys[62] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion3);
            FunctionKeys[63] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion4);
            FunctionKeys[64] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion5);
            FunctionKeys[65] = new CharacterFunctionKey(KeyType.BasicEmotion, KeyMenu.Emotion6);

            QuickSlotKeys = new CharacterQuickSlotKey[8];
            QuickSlotKeys[0] = new CharacterQuickSlotKey(42);
            QuickSlotKeys[1] = new CharacterQuickSlotKey(82);
            QuickSlotKeys[2] = new CharacterQuickSlotKey(71);
            QuickSlotKeys[3] = new CharacterQuickSlotKey(73);
            QuickSlotKeys[4] = new CharacterQuickSlotKey(29);
            QuickSlotKeys[5] = new CharacterQuickSlotKey(83);
            QuickSlotKeys[6] = new CharacterQuickSlotKey(79);
            QuickSlotKeys[7] = new CharacterQuickSlotKey(81);

            Inventories = new Dictionary<ItemInventoryType, ItemInventory>
            {
                [ItemInventoryType.Equip] = new ItemInventory(24),
                [ItemInventoryType.Consume] = new ItemInventory(24),
                [ItemInventoryType.Install] = new ItemInventory(24),
                [ItemInventoryType.Etc] = new ItemInventory(24),
                [ItemInventoryType.Cash] = new ItemInventory(24)
            };
            SkillRecord = new Dictionary<int, CharacterSkillRecord>();
            //QuestRecord = new Dictionary<short, string>();
            //QuestRecordEx = new Dictionary<short, string>();
            //QuestComplete = new Dictionary<short, DateTime>();

            WishList = new int[10];
        }
    }
}