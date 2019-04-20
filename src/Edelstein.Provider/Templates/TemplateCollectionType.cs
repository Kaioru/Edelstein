using System;

namespace Edelstein.Provider.Templates
{
    [Flags]
    public enum TemplateCollectionType
    {
        All = int.MaxValue,
        
        Item = 0x1,
        Field = 0x2,
        MakeCharInfo = 0x4,
        
        Login = Item | MakeCharInfo,
        Game = Item | Field
    }
}