namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

[Flags]
public enum QuestJobFlags
{
    Novice = 0x1,
    Swordman = 0x2,
    Magician = 0x4,
    Archer = 0x8,
    Rogue = 0x10,
    Pirate = 0x20,
    
    Noblesse = 0x400,
    Soulfighter = 0x800,
    Flamewizard = 0x1000,
    Windbreaker = 0x2000,
    Nightwalker = 0x4000,
    Striker = 0x8000,
    
    Legend = 0x100000,
    Aran = 0x200000,
    Evan = 0x400000
}
