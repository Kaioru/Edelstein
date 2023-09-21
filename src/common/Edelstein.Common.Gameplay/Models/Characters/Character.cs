using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record Character : ICharacter
{
    public int ID { get; set; }
    public int AccountWorldID { get; set; }

    public string Name { get; set; }
    public byte Gender { get; set; }
    public byte Skin { get; set; }
    public int Face { get; set; }
    public int Hair { get; set; }

    public byte Level { get; set; } = 1;
    public short Job { get; set; }
    public short STR { get; set; } = 4;
    public short DEX { get; set; } = 4;
    public short INT { get; set; } = 4;
    public short LUK { get; set; } = 4;

    public int HP { get; set; } = 50;
    public int MaxHP { get; set; } = 50;
    public int MP { get; set; } = 50;
    public int MaxMP { get; set; } = 50;

    public short AP { get; set; }
    public short SP { get; set; }

    public ICharacterExtendSP ExtendSP { get; private set; } = new CharacterExtendSP();

    public int EXP { get; set; }
    public short POP { get; set; }

    public int Money { get; set; }
    public int TempEXP { get; set; }

    public int FieldID { get; set; }
    public byte FieldPortal { get; set; }

    public int PlayTime { get; set; }

    public short SubJob { get; set; }

    public byte FriendMax { get; set; } = 20;

    public ICharacterFuncKeys FuncKeys { get; private set; } = new CharacterFuncKeys();
    public ICharacterQuickslotKeys QuickslotKeys { get; private set; } = new CharacterQuickslotKeys();
    
    public ICharacterWishlist Wishlist { get; private set; } = new CharacterWishlist();

    public ICharacterInventories Inventories { get; private set; } = new CharacterInventories();
    public ICharacterSkills Skills { get; private set; } = new CharacterSkills();
    public ICharacterQuestCompletes QuestCompletes { get; private set; } = new CharacterQuestCompletes();
    public ICharacterQuestRecords QuestRecords { get; private set; } = new CharacterQuestRecords();
    public ICharacterQuestRecordsEx QuestRecordsEx { get; private set; } = new CharacterQuestRecordsEx();
    
    public ICharacterWildHunterInfo WildHunterInfo { get; private set; } = new CharacterWildHunterInfo();

    public ICharacterTemporaryStats TemporaryStats { get; private set; } = new CharacterTemporaryStats();
}
