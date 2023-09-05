using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats.Modify;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats.Modify;

public class ModifyMobTemporaryStatsContext : IModifyMobTemporaryStatContext
{
    private readonly IMobTemporaryStats _stats;

    public ModifyMobTemporaryStatsContext(IMobTemporaryStats stats)
    {
        _stats = stats;
        HistoryReset = new MobTemporaryStats();
        HistorySet = new MobTemporaryStats();
    }
    
    public IMobTemporaryStats HistoryReset { get; }
    public IMobTemporaryStats HistorySet { get; }
    
    public void Set(MobTemporaryStatType type, IMobTemporaryStatRecord stat)
    {
        ResetByType(type);

        _stats.Records[type] = stat;
        HistorySet.Records[type] = stat;
    }

    public void Set(MobTemporaryStatType type, int value, int reason, DateTime? dateExpire = null)
        => Set(type, new MobTemporaryStatRecord
        {
            Value = value,
            Reason = reason,
            DateExpire = dateExpire
        });
    
    public void ResetByType(MobTemporaryStatType type)
    {
        if (_stats.Records.ContainsKey(type))
            HistoryReset.Records[type] = _stats.Records[type];
        _stats.Records.Remove(type);
    }

    public void ResetByReason(int reason)
    {
        foreach (var type in _stats.Records
                     .Where(kv => kv.Value.Reason == reason)
                     .Select(kv => kv.Key)
                     .ToImmutableList())
            ResetByType(type);
    }

    public void ResetAll()
    {
        foreach (var type in _stats.Records.Keys)
            ResetByType(type);
    }
}
