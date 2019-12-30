using System;

namespace Edelstein.Core.Gameplay
{
    [Flags]
    public enum DbChar
    {
        Character = 0x1,
        Money = 0x2,
        ItemSlotEquip = 0x4,
        ItemSlotConsume = 0x8,
        ItemSlotInstall = 0x10,
        ItemSlotEtc = 0x20,
        ItemSlotCash = 0x40,
        InventorySize = 0x80,
        SkillRecord = 0x100,
        QuestRecord = 0x200,
        MinigameRecord = 0x400,
        CoupleRecord = 0x800,
        MapTransfer = 0x1000,
        Avatar = 0x2000,
        QuestComplete = 0x4000,
        SkillCooltime = 0x8000,
        MonsterBookCard = 0x10000,
        MonsterBookCover = 0x20000,
        NewYearCard = 0x40000,
        QuestRecordEx = 0x80000,
        AdminShopCount = 0x100000,
        EquipExt = 0x100000,
        WildHunterInfo = 0x200000,
        QuestCompleteOld = 0x400000,
        VisitorLog = 0x800000,
        VisitorLog1 = 0x1000000,
        VisitorLog2 = 0x2000000,
        VisitorLog3 = 0x4000000,
        VisitorLog4 = 0x8000000,
        All = -1,
        ItemSlot = 0x7C
    }
}