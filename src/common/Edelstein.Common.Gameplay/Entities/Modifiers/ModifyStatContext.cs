using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Modifiers;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Entities.Modifiers;

public class ModifyStatContext(
    Character character    
) : IModifyStatContext
{
    public ModifyStatType Flag { get; set; }
    
    public byte Skin
    {
        get => character.Skin;
        set
        {
            Flag |= ModifyStatType.Skin;
            character.Skin = value;
        }
    }

    public int Face
    {
        get => character.Face;
        set
        {
            Flag |= ModifyStatType.Face;
            character.Face = value;
        }
    }

    public int Hair
    {
        get => character.Hair;
        set
        {
            Flag |= ModifyStatType.Hair;
            character.Hair = value;
        }
    }

    public byte Level
    {
        get => character.Level;
        set
        {
            Flag |= ModifyStatType.Level;
            character.Level = value;
        }
    }

    public short Job
    {
        get => character.Job;
        set
        {
            Flag |= ModifyStatType.Job;
            character.Job = value;
        }
    }

    public short STR
    {
        get => character.STR;
        set
        {
            Flag |= ModifyStatType.STR;
            character.STR = value;
        }
    }

    public short DEX
    {
        get => character.DEX;
        set
        {
            Flag |= ModifyStatType.DEX;
            character.DEX = value;
        }
    }

    public short INT
    {
        get => character.INT;
        set
        {
            Flag |= ModifyStatType.INT;
            character.INT = value;
        }
    }

    public short LUK
    {
        get => character.LUK;
        set
        {
            Flag |= ModifyStatType.LUK;
            character.LUK = value;
        }
    }

    public int HP
    {
        get => character.HP;
        set
        {
            Flag |= ModifyStatType.HP;
            character.HP = value;
        }
    }

    public int MaxHP
    {
        get => character.MaxHP;
        set
        {
            Flag |= ModifyStatType.MaxHP;
            character.MaxHP = value;
        }
    }

    public int MP
    {
        get => character.MP;
        set
        {
            Flag |= ModifyStatType.MP;
            character.MP = value;
        }
    }

    public int MaxMP
    {
        get => character.MaxMP;
        set
        {
            Flag |= ModifyStatType.MaxMP;
            character.MaxMP = value;
        }
    }

    public short AP
    {
        get => character.AP;
        set
        {
            Flag |= ModifyStatType.AP;
            character.AP = value;
        }
    }

    public short SP
    {
        get => character.SP;
        set
        {
            Flag |= ModifyStatType.SP;
            character.SP = value;
        }
    }

    public int EXP
    {
        get => character.EXP;
        set
        {
            Flag |= ModifyStatType.EXP;
            character.EXP = value;
        }
    }

    public short POP
    {
        get => character.POP;
        set
        {
            Flag |= ModifyStatType.POP;
            character.POP = value;
        }
    }

    public int Money
    {
        get => character.Money;
        set
        {
            Flag |= ModifyStatType.Money;
            character.Money = value;
        }
    }

    public int TempEXP
    {
        get => character.TempEXP;
        set
        {
            Flag |= ModifyStatType.TempEXP;
            character.TempEXP = value;
        }
    }

    public StructuredModifyStat GetDispatch()
        => new()
        {
            Skin = Flag.HasFlag(ModifyStatType.Skin) ? Skin : null,
            Face = Flag.HasFlag(ModifyStatType.Face) ? Face : null,
            Hair = Flag.HasFlag(ModifyStatType.Hair) ? Hair : null,
            Level = Flag.HasFlag(ModifyStatType.Level) ? Level : null,
            Job = Flag.HasFlag(ModifyStatType.Job) ? Job : null,
            STR = Flag.HasFlag(ModifyStatType.STR) ? STR : null,
            DEX = Flag.HasFlag(ModifyStatType.DEX) ? DEX : null,
            INT = Flag.HasFlag(ModifyStatType.INT) ? INT : null,
            LUK = Flag.HasFlag(ModifyStatType.LUK) ? LUK : null,
            HP = Flag.HasFlag(ModifyStatType.HP) ? HP : null,
            MaxHP = Flag.HasFlag(ModifyStatType.MaxHP) ? MaxHP : null,
            MP = Flag.HasFlag(ModifyStatType.MP) ? MP : null,
            MaxMP = Flag.HasFlag(ModifyStatType.MaxMP) ? MaxMP : null,
            AP = Flag.HasFlag(ModifyStatType.AP) ? AP : null,
            SP = Flag.HasFlag(ModifyStatType.SP) ? SP : null,
            EXP = Flag.HasFlag(ModifyStatType.EXP) ? EXP : null,
            POP = Flag.HasFlag(ModifyStatType.POP) ? POP : null,
            Money = Flag.HasFlag(ModifyStatType.Money) ? Money : null,
            TempEXP = Flag.HasFlag(ModifyStatType.TempEXP) ? TempEXP : null,
        };
}
