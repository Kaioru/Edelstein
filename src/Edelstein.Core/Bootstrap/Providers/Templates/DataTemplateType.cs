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
        Mob = 0x8000,
        Reactor = 0x10000,
        ItemOption = 0x10,
        SetItemInfo = 0x20,

        Continent = 0x40,

        ItemString = 0x80,
        FieldString = 0x100,

        Commodity = 0x200,
        CashPackage = 0x400,
        ModifiedCommodity = 0x800,
        Best = 0x1000,
        CategoryDiscount = 0x2000,
        NotSale = 0x4000,

        Login = Item |
                MakeCharInfo,

        Game = Item |
               Field |
               NPC |
               Mob |
               Reactor |
               ItemOption |
               SetItemInfo |
               Continent |
               ItemString |
               FieldString,

        Shop = Item |
               Commodity |
               CashPackage |
               ModifiedCommodity |
               Best |
               CategoryDiscount |
               NotSale,

        Trade = Item
    }
}