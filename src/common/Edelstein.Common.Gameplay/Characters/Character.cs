using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Common.Gameplay.Characters;

public record Character : ICharacter
{
    public int ID { get; set; }

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

    public IDictionary<byte, byte> ExtendSP { get; }

    public int EXP { get; set; }
    public short POP { get; set; }

    public int Money { get; set; }
    public int TempEXP { get; set; }

    public int FieldID { get; set; }
    public byte FieldPortal { get; set; }

    public int PlayTime { get; set; }

    public short SubJob { get; set; }

    public IDictionary<ItemInventoryType, IItemInventory> Inventories { get; }

    public int[] WishList { get; }
}
