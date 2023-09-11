using Edelstein.Common.Utilities;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats;

public static class CharacterTemporaryStatsPackets
{
    private const int TemporaryStatsFlagSize = 128;

    public static void WriteTemporaryStatsFlag(this IPacketWriter writer, ICharacterTemporaryStats stats)
    {
        var flag = new Flags(TemporaryStatsFlagSize);

        foreach (var type in stats.Records.Keys) 
            flag.SetFlag((int)type);

        if (stats.EnergyChargedRecord != null)
            flag.SetFlag((int)TemporaryStatType.EnergyCharged);
        if (stats.DashSpeedRecord != null)
            flag.SetFlag((int)TemporaryStatType.Dash_Speed);
        if (stats.DashJumpRecord != null)
            flag.SetFlag((int)TemporaryStatType.Dash_Jump);
        if (stats.RideVehicleRecord != null)
            flag.SetFlag((int)TemporaryStatType.RideVehicle);
        if (stats.PartyBoosterRecord != null)
            flag.SetFlag((int)TemporaryStatType.PartyBooster);
        if (stats.GuidedBulletRecord != null)
            flag.SetFlag((int)TemporaryStatType.GuidedBullet);
        if (stats.UndeadRecord != null)
            flag.SetFlag((int)TemporaryStatType.Undead);
        
        writer.Write(flag);
    }

    public static void WriteTemporaryStatsToLocal(this IPacketWriter writer, ICharacterTemporaryStats stats)
    {
        var now = DateTime.UtcNow;

        writer.WriteTemporaryStatsFlag(stats);

        foreach (var type in CharacterTemporaryStatsOrder.WriteOrderLocal)
        {
            var stat = stats[type];
            if (stat == null) continue;
            var remaining = stat.DateExpire.HasValue 
                ? (stat.DateExpire.Value - now).TotalMilliseconds 
                : int.MaxValue;
            
            writer.WriteShort((short)stat.Value);
            writer.WriteInt(stat.Reason);
            writer.WriteInt((int)remaining);
        }
        writer.WriteByte(0); // nDefenseAtt
        writer.WriteByte(0); // nDefenseState

        if (
            stats[TemporaryStatType.SwallowAttackDamage] != null &&
            stats[TemporaryStatType.SwallowDefence] != null &&
            stats[TemporaryStatType.SwallowCritical] != null &&
            stats[TemporaryStatType.SwallowMaxMP] != null &&
            stats[TemporaryStatType.SwallowEvasion] != null
        )
            writer.WriteByte(0);

        if (stats[TemporaryStatType.Dice] != null)
            writer.WriteBytes(new byte[22]);

        if (stats[TemporaryStatType.BlessingArmor] != null)
            writer.WriteInt(0);
        
        writer.WriteTwoStateTemporaryStats(stats, now);
    }

    public static void WriteTemporaryStatsToRemote(this IPacketWriter writer, ICharacterTemporaryStats stats)
    {
        writer.WriteTemporaryStatsFlag(stats);

        foreach (var kv in CharacterTemporaryStatsOrder.WriteOrderRemote)
        {
            var stat = stats[kv.Key];
            if (stat == null) continue;
            kv.Value.Invoke(stat, writer);
        }
        
        writer.WriteByte(0); // nDefenseAtt
        writer.WriteByte(0); // nDefenseState

        writer.WriteTwoStateTemporaryStats(stats, DateTime.UtcNow);
    }

    private static void WriteTwoStateTemporaryStats(this IPacketWriter writer, ICharacterTemporaryStats stats, DateTime now)
    {
        if (stats.EnergyChargedRecord != null) writer.WriteTwoStateRecordDynamicTerm(stats.EnergyChargedRecord, now);
        if (stats.DashSpeedRecord != null) writer.WriteTwoStateRecordDynamicTerm(stats.DashSpeedRecord, now);
        if (stats.DashJumpRecord != null) writer.WriteTwoStateRecordDynamicTerm(stats.DashJumpRecord, now);
        if (stats.RideVehicleRecord != null) writer.WriteTwoStateRecord(stats.RideVehicleRecord, now);

        if (stats.PartyBoosterRecord != null)
        {
            writer.WriteTwoStateRecord(stats.PartyBoosterRecord, now);
            writer.WriteTwoStateTime(stats.PartyBoosterRecord.DateStart, now);
            writer.WriteShort((short)stats.PartyBoosterRecord.Term.TotalSeconds);
        }

        if (stats.GuidedBulletRecord != null)
        {
            writer.WriteTwoStateRecord(stats.GuidedBulletRecord, now);
            writer.WriteInt(stats.GuidedBulletRecord.MobID);
        }
        
        if (stats.UndeadRecord != null) writer.WriteTwoStateRecordDynamicTerm(stats.UndeadRecord, now);
    }

    private static void WriteTwoStateTime(this IPacketWriter writer, DateTime time, DateTime now)
    {
        writer.WriteBool(time < now);
        writer.WriteInt((short)(time - now).TotalSeconds);
    }

    private static void WriteTwoStateRecord(this IPacketWriter writer, ITwoStateTemporaryStatRecord record, DateTime now)
    {
        writer.WriteInt(record.Value);
        writer.WriteInt(record.Reason);
        writer.WriteTwoStateTime(record.DateUpdated, now);
    }
    
    private static void WriteTwoStateRecordDynamicTerm(this IPacketWriter writer, ITwoStateTemporaryStatRecordDynamicTerm record, DateTime now)
    {
        writer.WriteTwoStateRecord(record, now);
        writer.WriteShort((short)record.Term.TotalSeconds);
    }
}
