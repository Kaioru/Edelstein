using Edelstein.Common.Utilities;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public static class MobTemporaryStatsPackets
{
    private const int MobStatsFlagSize = 128;

    public static void WriteMobTemporaryStatsFlag(this IPacketWriter writer, IMobTemporaryStats mobStats)
    {
        var flag = new Flags(MobStatsFlagSize);

        foreach (var type in mobStats.Records.Keys) 
            flag.SetFlag((int)type);
        
        if (mobStats.BurnedInfo.Count > 0)
            flag.SetFlag((int)MobTemporaryStatType.Burned);

        writer.Write(flag);
    }

    public static void WriteMobTemporaryStats(this IPacketWriter writer, IMobTemporaryStats mobStats)
    {
        var stats = mobStats.Records;
        var now = DateTime.UtcNow;
        
        writer.WriteMobTemporaryStatsFlag(mobStats);
        
        foreach (var t in MobTemporaryStatsOrder.WriteOrder)
        {
            if (!stats.ContainsKey(t)) continue;

            var stat = stats[t];
            var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : short.MaxValue;

            writer.WriteShort((short)stat.Value);
            writer.WriteInt(stat.Reason);
            writer.WriteShort((short)remaining);
        }

        if (mobStats.BurnedInfo.Count > 0)
        {
            writer.WriteInt(mobStats.BurnedInfo.Count);
            
            foreach (var burned in mobStats.BurnedInfo)
            {
                writer.WriteInt(burned.CharacterID);
                writer.WriteInt(burned.SkillID);
                writer.WriteInt(burned.Damage);
                writer.WriteInt((int)burned.Interval.TotalMilliseconds);
                writer.WriteInt(0);
                writer.WriteInt((int)((burned.DateExpire - now).TotalMilliseconds / burned.Interval.TotalMilliseconds));
            }
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
