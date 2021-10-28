using System;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public static class SecondaryStatsPackets
    {
        private const int SecondaryStatFlagSize = 128;

        public static void WriteSecondaryStatsFlag(this IPacketWriter writer, ISecondaryStats stats)
        {
            var all = stats.All();
            var flag = new Flags(SecondaryStatFlagSize);

            all.Keys.ForEach(t => flag.SetFlag((int)t));

            writer.Write(flag);
        }

        public static void WriteSecondaryStatsToLocal(this IPacketWriter writer, ISecondaryStats stats)
        {
            var now = DateTime.UtcNow;
            var all = stats.All();

            writer.WriteSecondaryStatsFlag(stats);

            SecondaryStatsOrder.WriteOrderLocal.ForEach(t =>
            {
                if (!all.ContainsKey(t)) return;

                var stat = all[t];
                var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : int.MaxValue;

                writer.WriteShort((short)stat.Value);
                writer.WriteInt(stat.Reason);
                writer.WriteInt((int)remaining);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            if (
                all.ContainsKey(SecondaryStatType.SwallowAttackDamage) &&
                all.ContainsKey(SecondaryStatType.SwallowDefence) &&
                all.ContainsKey(SecondaryStatType.SwallowCritical) &&
                all.ContainsKey(SecondaryStatType.SwallowMaxMP) &&
                all.ContainsKey(SecondaryStatType.SwallowEvasion)
            )
                writer.WriteByte(0);

            if (all.ContainsKey(SecondaryStatType.Dice))
                writer.WriteBytes(new byte[22]);

            if (all.ContainsKey(SecondaryStatType.BlessingArmor))
                writer.WriteInt(0);

            // TODO: TWOSTATE
        }

        public static void WriteSecondaryStatsToRemote(this IPacketWriter writer, ISecondaryStats stats)
        {
            var all = stats.All();

            writer.WriteSecondaryStatsFlag(stats);

            SecondaryStatsOrder.WriteOrderRemote.ForEach(kv =>
            {
                if (!all.ContainsKey(kv.Key)) return;
                kv.Value.Invoke(all[kv.Key], writer);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            // TODO: TWOSTATE
        }
    }
}
