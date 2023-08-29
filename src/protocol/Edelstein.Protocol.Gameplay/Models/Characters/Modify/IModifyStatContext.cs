using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Modify;

public interface IModifyStatContext : IPacketWritable
{
    ModifyStatType Flag { get; }

    byte Skin { get; set; }
    int Face { get; set; }
    int Hair { get; set; }

    // long Pet1 { get; set; }
    // long Pet2 { get; set; }
    // long Pet3 { get; set; }

    byte Level { get; set; }
    short Job { get; set; }

    short STR { get; set; }
    short DEX { get; set; }
    short INT { get; set; }
    short LUK { get; set; }

    int HP { get; set; }
    int MaxHP { get; set; }
    int MP { get; set; }
    int MaxMP { get; set; }

    short AP { get; set; }
    short SP { get; set; }

    int EXP { get; set; }
    short POP { get; set; }

    int Money { get; set; }
    int TempEXP { get; set; }

    void SetExtendSP(byte jobLevel, byte amount);
}
