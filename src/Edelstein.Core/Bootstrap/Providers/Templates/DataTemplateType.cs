using System;

namespace Edelstein.Core.Bootstrap.Providers.Templates
{
    [Flags]
    public enum DataTemplateType
    {
        All = int.MaxValue,
        None = 0x0,

        Item = 0x1,
        MakeCharInfo = 0x2,

        Field = 0x4,
        NPC = 0x8,

        Login = Item |
                MakeCharInfo,

        Game = Item |
               Field |
               NPC
    }
}