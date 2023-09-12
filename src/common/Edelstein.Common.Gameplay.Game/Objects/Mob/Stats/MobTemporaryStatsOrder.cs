using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

internal static class MobTemporaryStatsOrder
{
    internal static readonly MobTemporaryStatType[] WriteOrder =
    {
        MobTemporaryStatType.PAD,
        MobTemporaryStatType.PDR,
        MobTemporaryStatType.MAD,
        MobTemporaryStatType.MDR,
        MobTemporaryStatType.ACC,
        MobTemporaryStatType.EVA,
        MobTemporaryStatType.Speed,
        MobTemporaryStatType.Stun,
        MobTemporaryStatType.Freeze,
        MobTemporaryStatType.Poison,
        MobTemporaryStatType.Seal,
        MobTemporaryStatType.Darkness,
        MobTemporaryStatType.PowerUp,
        MobTemporaryStatType.MagicUp,
        MobTemporaryStatType.PGuardUp,
        MobTemporaryStatType.MGuardUp,
        MobTemporaryStatType.PImmune,
        MobTemporaryStatType.MImmune,
        MobTemporaryStatType.Doom,
        MobTemporaryStatType.Web,
        MobTemporaryStatType.HardSkin,
        MobTemporaryStatType.Ambush,
        MobTemporaryStatType.Venom,
        MobTemporaryStatType.Blind,
        MobTemporaryStatType.SealSkill,
        MobTemporaryStatType.Dazzle,
        MobTemporaryStatType.PCounter,
        MobTemporaryStatType.MCounter,
        MobTemporaryStatType.RiseByToss,
        MobTemporaryStatType.BodyPressure,
        MobTemporaryStatType.Weakness,
        MobTemporaryStatType.TimeBomb,
        MobTemporaryStatType.Showdown,
        MobTemporaryStatType.MagicCrash,
        MobTemporaryStatType.DamagedElemAttr,
        MobTemporaryStatType.HealByDamage
    };
}
