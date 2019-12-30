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

        Login = Item |
                MakeCharInfo
    }
}