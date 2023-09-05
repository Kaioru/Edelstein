using Edelstein.Common.Utilities;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public static class MobTemporaryStatsPackets
{
    private const int MobStatsFlagSize = 128;

    internal static void WriteMobStatsFlag(this IPacketWriter writer, IDictionary<MobTemporaryStatType, IMobTemporaryStatRecord> stats)
    {
        var flag = new Flags(MobStatsFlagSize);

        foreach (var type in stats.Keys) 
            flag.SetFlag((int)type);

        writer.Write(flag);
    }

    public static void WriteMobStatsFlag(this IPacketWriter writer, IMobTemporaryStats mobStats) 
        => writer.WriteMobStatsFlag(mobStats.Records);

    public static void WriteMobStats(this IPacketWriter writer, IMobTemporaryStats mobStats)
    {
        var stats = mobStats.Records;
        var now = DateTime.UtcNow;

        writer.WriteMobStatsFlag(mobStats);
        
        foreach (var t in MobTemporaryStatsOrder.WriteOrder)
        {
            if (!stats.ContainsKey(t)) continue;

            var stat = stats[t];
            var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : short.MaxValue;

            writer.WriteShort((short)stat.Value);
            writer.WriteInt(stat.Reason);
            writer.WriteShort((short)remaining);
        }

        if (stats.ContainsKey(MobTemporaryStatType.Burned))
        {
            writer.WriteInt(0); // Count
            writer.WriteInt(0); // CharacterID
            writer.WriteInt(0); // SkillID
            writer.WriteInt(0); // Damage
            writer.WriteInt(0); // Interval
            writer.WriteInt(0); // End
            writer.WriteInt(0); // DotCount
        }

        if (stats.ContainsKey(MobTemporaryStatType.PCounter))
            writer.WriteInt(0); // ModValue?

        if (stats.ContainsKey(MobTemporaryStatType.MCounter))
            writer.WriteInt(0); // ModValue?

        if (stats.ContainsKey(MobTemporaryStatType.PCounter) || stats.ContainsKey(MobTemporaryStatType.MCounter))
            writer.WriteInt(100); // CounterProb

        if (stats.ContainsKey(MobTemporaryStatType.Disable))
        {
            writer.WriteBool(true); // Invincible
            writer.WriteBool(false); // Disable
        }
    }
}
