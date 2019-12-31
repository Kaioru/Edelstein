using System;

namespace Edelstein.Core.Templates.Fields
{
    [Flags]
    public enum FieldOpt
    {
        MoveLimit = 0x1,
        SkillLimit = 0x2,
        SummonLimit = 0x4,
        MysticDoorLimit = 0x8,
        MigrateLimit = 0x10,
        PortalScrollLimit = 0x20,
        TeleportItemLimit = 0x40,
        MinigameLimit = 0x80,
        SpecificPortalScrollLimit = 0x100,
        TamingMobLimit = 0x200,
        StatChangeItemConsumeLimit = 0x400,
        PartyBossChangeLimit = 0x800,
        NoMobCapacityLimit = 0x1000,
        WeddingInvitationLimit = 0x2000,
        CashWeatherConsumeLimit = 0x4000,
        NoPet = 0x8000,
        AntiMacroLimit = 0x10000,
        FalldownLimit = 0x20000,
        SummonNPCLimit = 0x40000,
        NoEXPDecrease = 0x80000,
        NoDamageOnFalling = 0x100000,
        PacelOpenLimit = 0x200000,
        DropLimit = 0x400000,
        RocketBoosterLimit = 0x800000
    }
}