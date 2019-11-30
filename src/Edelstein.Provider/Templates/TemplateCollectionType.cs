using System;

namespace Edelstein.Provider.Templates
{
    [Flags]
    public enum TemplateCollectionType : long
    {
        All = int.MaxValue,
        None = 0x0,

        Item = 0x1,
        Field = 0x2,
        NPC = 0x4,
        MakeCharInfo = 0x8,
        Commodity = 0x10,
        CashPackage = 0x20,
        ModifiedCommodity = 0x40,
        Best = 0x80,
        CategoryDiscount = 0x100,
        NotSale = 0x200,
        SetItemInfo = 0x400,
        ItemOption = 0x800,
        Mob = 0x1000,
        Continent = 0x2000,
        Reactor = 0x4000,
        Skill = 0x8000,
        Quest = 0x100000,
        Reward = 0x200000,

        FieldString = 0x10000,
        ItemString = 0x20000,
        SkillString = 0x40000,
        QuestString = 0x80000,
        MobString = 0x400000,
        String = FieldString | ItemString | SkillString | QuestString | MobString,

        NPCShop = 0x200000,

        Login = Item | 
                MakeCharInfo,

        Game = Item |
               Field |
               NPC |
               SetItemInfo |
               ItemOption |
               Mob |
               Continent |
               Reactor |
               Skill |
               Quest |
               String |
               NPCShop |
               Reward,
        Shop = Item | 
               Commodity | 
               CashPackage | 
               ModifiedCommodity | 
               Best | 
               CategoryDiscount | 
               NotSale
    }
}