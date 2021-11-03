using System;
using System.Collections.Generic;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public static class SecondaryStatsPackets
    {
        private const int SecondaryStatsFlagSize = 128;

        internal static void WriteSecondaryStatsFlag(this IPacketWriter writer, IDictionary<SecondaryStatType, ITemporaryStat> stats)
        {
            var flag = new Flags(SecondaryStatsFlagSize);

            stats.Keys.ForEach(t => flag.SetFlag((int)t));

            writer.Write(flag);
        }

        public static void WriteSecondaryStatsFlag(this IPacketWriter writer, ISecondaryStats secondary)
            => writer.WriteSecondaryStatsFlag(secondary.ToDictionary());

        public static void WriteSecondaryStatsToLocal(this IPacketWriter writer, ISecondaryStats secondary)
        {
            var stats = secondary.ToDictionary();
            var now = DateTime.UtcNow;

            writer.WriteSecondaryStatsFlag(stats);

            SecondaryStatsOrder.WriteOrderLocal.ForEach(t =>
            {
                if (!secondary.HasStat(t)) return;

                var stat = stats[t];
                var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : int.MaxValue;

                writer.WriteShort((short)stat.Value);
                writer.WriteInt(stat.Reason);
                writer.WriteInt((int)remaining);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            if (
                secondary.HasStat(SecondaryStatType.SwallowAttackDamage) &&
                secondary.HasStat(SecondaryStatType.SwallowDefence) &&
                secondary.HasStat(SecondaryStatType.SwallowCritical) &&
                secondary.HasStat(SecondaryStatType.SwallowMaxMP) &&
                secondary.HasStat(SecondaryStatType.SwallowEvasion)
            )
                writer.WriteByte(0);

            if (secondary.HasStat(SecondaryStatType.Dice))
                writer.WriteBytes(new byte[22]);

            if (secondary.HasStat(SecondaryStatType.BlessingArmor))
                writer.WriteInt(0);

            // TODO: TWOSTATE
        }

        public static void WriteSecondaryStatsToRemote(this IPacketWriter writer, ISecondaryStats secondary)
        {
            var stats = secondary.ToDictionary();

            writer.WriteSecondaryStatsFlag(stats);

            SecondaryStatsOrder.WriteOrderRemote.ForEach(kv =>
            {
                if (!secondary.HasStat(kv.Key)) return;
                kv.Value.Invoke(stats[kv.Key], writer);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            // TODO: TWOSTATE
        }
    }
}
