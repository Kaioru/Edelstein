using System;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public static class MobStatsPackets
    {
        private const int MobStatsFlagSize = 128;

        public static void WriteMobStatsFlag(this IPacketWriter writer, IMobStats mobStats)
        {
            var flag = new Flags(MobStatsFlagSize);

            mobStats.Stats.Keys.ForEach(t => flag.SetFlag((int)t));

            writer.Write(flag);
        }

        public static void WriteMobStats(this IPacketWriter writer, IMobStats mobStats)
        {
            var now = DateTime.UtcNow;

            writer.WriteMobStatsFlag(mobStats);

            MobStatsOrder.WriteOrder.ForEach(t =>
            {
                if (!mobStats.Stats.ContainsKey(t)) return;

                var stat = mobStats.Stats[t];
                var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : int.MaxValue;

                writer.WriteShort((short)stat.Value);
                writer.WriteInt(stat.Reason);
                writer.WriteShort((short)remaining);
            });

            if (mobStats.Stats.ContainsKey(MobStatType.Burned))
            {
                writer.WriteInt(0); // Count
                writer.WriteInt(0); // CharacterID
                writer.WriteInt(0); // SkillID
                writer.WriteInt(0); // Damage
                writer.WriteInt(0); // Interval
                writer.WriteInt(0); // End
                writer.WriteInt(0); // DotCount
            }

            if (mobStats.Stats.ContainsKey(MobStatType.PCounter))
                writer.WriteInt(0); // ModValue?

            if (mobStats.Stats.ContainsKey(MobStatType.MCounter))
                writer.WriteInt(0); // ModValue?

            if (mobStats.Stats.ContainsKey(MobStatType.PCounter) || mobStats.Stats.ContainsKey(MobStatType.MCounter))
                writer.WriteInt(100); // CounterProb

            if (mobStats.Stats.ContainsKey(MobStatType.Disable))
            {
                writer.WriteBool(true); // Invincible
                writer.WriteBool(false); // Disable
            }
        }
    }
}