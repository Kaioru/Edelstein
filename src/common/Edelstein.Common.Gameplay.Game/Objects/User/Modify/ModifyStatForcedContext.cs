using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Modify;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Modify;

public class ModifyStatForcedContext : IModifyStatForcedContext
{
    public ModifyStatForcedType Flag { get; private set; }
    
    public bool IsReset { get; private set; }

    public short STR
    {
        get => _stats.STR ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.STR;
            _stats.STR = value;
        }
    }
    public short DEX
    {
        get => _stats.DEX ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.DEX;
            _stats.DEX = value;
        }
    }
    public short INT
    {
        get => _stats.INT ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.INT;
            _stats.INT = value;
        }
    }
    public short LUK
    {
        get => _stats.LUK ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.LUK;
            _stats.LUK = value;
        }
    }

    public short PAD
    {
        get => _stats.PAD ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.PAD;
            _stats.PAD = value;
        }
    }
    public short PDD
    {
        get => _stats.PDD ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.PDD;
            _stats.PDD = value;
        }
    }

    public short MAD
    {
        get => _stats.MAD ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.MAD;
            _stats.MAD = value;
        }
    }
    public short MDD
    {
        get => _stats.MDD ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.MDD;
            _stats.MDD = value;
        }
    }

    public short ACC
    {
        get => _stats.ACC ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.ACC;
            _stats.ACC = value;
        }
    }
    public short EVA
    {
        get => _stats.EVA ?? 0;
        set
        {
            Flag |= ModifyStatForcedType.EVA;
            _stats.EVA = value;
        }
    }

    public byte Speed
    {
        get => _stats.Speed ?? 100;
        set
        {
            Flag |= ModifyStatForcedType.Speed;
            _stats.Speed = value;
        }
    }
    public byte Jump
    {
        get => _stats.Jump ?? 100;
        set
        {
            Flag |= ModifyStatForcedType.Jump;
            _stats.Jump = value;
        }
    }

    public byte SpeedMax
    {
        get => _stats.SpeedMax ?? 140;
        set
        {
            Flag |= ModifyStatForcedType.SpeedMax;
            _stats.SpeedMax = value;
        }
    }

    private readonly IFieldUserStatsForced _stats;
    
    public ModifyStatForcedContext(IFieldUserStatsForced stats) => _stats = stats;

    public void Reset()
    {
        Flag = 0;
        
        IsReset = true;
        
        _stats.STR = null;
        _stats.DEX = null;
        _stats.INT = null;
        _stats.LUK = null;
        
        _stats.PAD = null;
        _stats.PDD = null;
        _stats.MAD = null;
        _stats.MDD = null;
        _stats.EVA = null;
        _stats.ACC = null;
        
        _stats.Speed = null;
        _stats.Jump = null;
        
        _stats.SpeedMax = null;
    }
}
