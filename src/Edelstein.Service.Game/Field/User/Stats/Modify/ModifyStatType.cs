using System;

namespace Edelstein.Service.Game.Field.User.Stats.Modify
{
    [Flags]
    public enum ModifyStatType
    {
        Skin = 0x1,
        Face = 0x2,
        Hair = 0x4,

        Pet = 0x8,
        Pet2 = 0x80000,
        Pet3 = 0x100000,

        Level = 0x10,
        Job = 0x20,
        STR = 0x40,
        DEX = 0x80,
        INT = 0x100,
        LUK = 0x200,

        HP = 0x400,
        MaxHP = 0x800,
        MP = 0x1000,
        MaxMP = 0x2000,

        AP = 0x4000,
        SP = 0x8000,

        EXP = 0x10000,
        POP = 0x20000,

        Money = 0x40000,
        TempEXP = 0x200000
    }
}