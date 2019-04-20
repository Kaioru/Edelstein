using System;

namespace Edelstein.Provider.Templates
{
    [Flags]
    public enum TemplateCollectionType
    {
        All = int.MaxValue,
        
        Item = 0x1,
        
        Login = Item,
        Game = Item
    }
}