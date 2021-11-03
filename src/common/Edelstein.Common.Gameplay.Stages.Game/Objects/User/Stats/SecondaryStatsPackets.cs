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

        public static void WriteSecondaryStatsFlag(this IPacketWriter writer, ISecondaryStats secondary)
        {
            var flag = new Flags(SecondaryStatFlagSize);

            secondary.Stats.Keys.ForEach(t => flag.SetFlag((int)t));

            writer.Write(flag);
        }

        public static void WriteSecondaryStatsToLocal(this IPacketWriter writer, ISecondaryStats secondary)
        {
            var now = DateTime.UtcNow;

            writer.WriteSecondaryStatsFlag(secondary);

            SecondaryStatsOrder.WriteOrderLocal.ForEach(t =>
            {
                if (!secondary.Stats.ContainsKey(t)) return;

                var stat = secondary.Stats[t];
                var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : int.MaxValue;

                writer.WriteShort((short)stat.Value);
                writer.WriteInt(stat.Reason);
                writer.WriteInt((int)remaining);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            if (
                secondary.Stats.ContainsKey(SecondaryStatType.SwallowAttackDamage) &&
                secondary.Stats.ContainsKey(SecondaryStatType.SwallowDefence) &&
                secondary.Stats.ContainsKey(SecondaryStatType.SwallowCritical) &&
                secondary.Stats.ContainsKey(SecondaryStatType.SwallowMaxMP) &&
                secondary.Stats.ContainsKey(SecondaryStatType.SwallowEvasion)
            )
                writer.WriteByte(0);

            if (secondary.Stats.ContainsKey(SecondaryStatType.Dice))
                writer.WriteBytes(new byte[22]);

            if (secondary.Stats.ContainsKey(SecondaryStatType.BlessingArmor))
                writer.WriteInt(0);

            // TODO: TWOSTATE
        }

        public static void WriteSecondaryStatsToRemote(this IPacketWriter writer, ISecondaryStats secondary)
        {
            writer.WriteSecondaryStatsFlag(secondary);

            SecondaryStatsOrder.WriteOrderRemote.ForEach(kv =>
            {
                if (!secondary.Stats.ContainsKey(kv.Key)) return;
                kv.Value.Invoke(secondary.Stats[kv.Key], writer);
            });

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState

            // TODO: TWOSTATE
        }
    }
}
