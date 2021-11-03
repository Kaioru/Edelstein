using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public static class MobStatsOrder
    {
        public static readonly MobStatType[] WriteOrder =
        {
            MobStatType.PAD,
            MobStatType.PDR,
            MobStatType.MAD,
            MobStatType.MDR,
            MobStatType.ACC,
            MobStatType.EVA,
            MobStatType.Speed,
            MobStatType.Stun,
            MobStatType.Freeze,
            MobStatType.Poison,
            MobStatType.Seal,
            MobStatType.Darkness,
            MobStatType.PowerUp,
            MobStatType.MagicUp,
            MobStatType.PGuardUp,
            MobStatType.MGuardUp,
            MobStatType.PImmune,
            MobStatType.MImmune,
            MobStatType.Doom,
            MobStatType.Web,
            MobStatType.HardSkin,
            MobStatType.Ambush,
            MobStatType.Venom,
            MobStatType.Blind,
            MobStatType.SealSkill,
            MobStatType.Dazzle,
            MobStatType.PCounter,
            MobStatType.MCounter,
            MobStatType.RiseByToss,
            MobStatType.BodyPressure,
            MobStatType.Weakness,
            MobStatType.TimeBomb,
            MobStatType.Showdown,
            MobStatType.MagicCrash,
            MobStatType.DamagedElemAttr,
            MobStatType.HealByDamage
        };
    }
}
