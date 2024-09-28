using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Entities;

public record Character : IRepositoryEntry<int>
{
    public int ID { get; set; }
    public int AccountWorldDataID { get; set; }
    
    public required string Name { get; set; }
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

    public int EXP { get; set; }
    public short POP { get; set; }

    public int Money { get; set; }
    public int TempEXP { get; set; }

    public int FieldID { get; set; }
    public byte FieldPortal { get; set; }

    public int PlayTime { get; set; }

    public short SubJob { get; set; }

    public CharacterInventories Inventories { get; set; } = new();
    public CharacterTemporaryStats TemporaryStats { get; set; } = new();
}
