using System;

namespace Edelstein.Provider.Templates
{
    [Flags]
    public enum TemplateCollectionType
    {
        All = int.MaxValue,
        
        Item = 0x1,
        MakeCharInfo = 0x2,
        
        Login = Item | MakeCharInfo,
        Game = Item
    }
}