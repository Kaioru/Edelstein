using System;

namespace Edelstein.Service.Game.Fields.User.Stats.Modify
{
    [Flags]
    public enum ModifyForcedStatType
    {
        STR = 0x1,
        DEX = 0x2,
        INT = 0x4,
        LUK = 0x8,
        PAD = 0x10,
        PDD = 0x20,
        MAD = 0x40,
        MDD = 0x80,
        ACC = 0x100,
        EVA = 0x200,
        Speed = 0x400,
        Jump = 0x800,
        SpeedMax = 0x1000
    }
}