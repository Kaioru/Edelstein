using Edelstein.Common.Utilities;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats;

public static class CharacterTemporaryStatsPackets
{
    private const int SecondaryStatsFlagSize = 128;

    private static void WriteTemporaryStatsFlag(this IPacketWriter writer, IDictionary<TemporaryStatType, ITemporaryStatRecord> stats)
    {
        var flag = new Flags(SecondaryStatsFlagSize);

        foreach (var type in stats.Keys) 
            flag.SetFlag((int)type);

        writer.Write(flag);
    }
    
    public static void WriteTemporaryStatsFlag(this IPacketWriter writer, ICharacterTemporaryStats stats)
        => writer.WriteTemporaryStatsFlag(stats.Records);

    public static void WriteTemporaryStatsToLocal(this IPacketWriter writer, ICharacterTemporaryStats stats)
    {
        var now = DateTime.UtcNow;

        writer.WriteTemporaryStatsFlag(stats);

        foreach (var type in CharacterTemporaryStatsOrder.WriteOrderLocal)
        {
            var stat = stats[type];
            if (stat == null) continue;
            var remaining = stat.DateExpire.HasValue ? (stat.DateExpire.Value - now).TotalMilliseconds : int.MaxValue;
            
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

        // TODO: TWOSTATE
    }
}
