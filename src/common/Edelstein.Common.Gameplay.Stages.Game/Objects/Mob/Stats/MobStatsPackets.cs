using System;
using System.Collections.Generic;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public static class MobStatsPackets
    {
        private const int MobStatsFlagSize = 128;

        internal static void WriteMobStatsFlag(this IPacketWriter writer, IDictionary<MobStatType, IMobStat> stats)
        {
            var flag = new Flags(MobStatsFlagSize);

            stats.Keys.ForEach(t => flag.SetFlag((int)t));

            writer.Write(flag);
        }

        public static void WriteMobStatsFlag(this IPacketWriter writer, IMobStats mobStats)
        {
            writer.WriteMobStatsFlag(mobStats.ToDictionary());
        }

        public static void WriteMobStats(this IPacketWriter writer, IMobStats mobStats)
        {
            var stats = mobStats.ToDictionary();
            var now = DateTime.UtcNow;

            writer.WriteMobStatsFlag(mobStats);

            MobStatsOrder.WriteOrder.ForEach(t =>
            {
                if (!stats.ContainsKey(t)) return;

                var stat = stats[t];
                var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : short.MaxValue;

                writer.WriteShort((short)stat.Value);
                writer.WriteInt(stat.Reason);
                writer.WriteShort((short)remaining);
            });

            if (stats.ContainsKey(MobStatType.Burned))
            {
                writer.WriteInt(0); // Count
                writer.WriteInt(0); // CharacterID
                writer.WriteInt(0); // SkillID
                writer.WriteInt(0); // Damage
                writer.WriteInt(0); // Interval
                writer.WriteInt(0); // End
                writer.WriteInt(0); // DotCount
            }

            if (stats.ContainsKey(MobStatType.PCounter))
                writer.WriteInt(0); // ModValue?

            if (stats.ContainsKey(MobStatType.MCounter))
                writer.WriteInt(0); // ModValue?

            if (stats.ContainsKey(MobStatType.PCounter) || stats.ContainsKey(MobStatType.MCounter))
                writer.WriteInt(100); // CounterProb

            if (stats.ContainsKey(MobStatType.Disable))
            {
                writer.WriteBool(true); // Invincible
                writer.WriteBool(false); // Disable
            }
        }
    }
}