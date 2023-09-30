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

    public void SetBurnedInfo(IMobBurnedInfo info)
    {
        var burned = _stats.BurnedInfo
            .FirstOrDefault(b => b.CharacterID == info.CharacterID && b.SkillID == info.SkillID);

        if (burned != null)
            ResetBurnedInfo(burned);
        
        _stats.BurnedInfo.Add(info);
        HistorySet.BurnedInfo.Add(info);
    }

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
                     .ToImmutableHashSet())
            ResetByType(type);
    }

    public void ResetAll()
    {
        foreach (var type in _stats.Records.Keys)
            ResetByType(type);
    }
    
    public void ResetBurnedInfo(IMobBurnedInfo info)
    {
        _stats.BurnedInfo.Remove(info);
        HistoryReset.BurnedInfo.Add(info);
    }

    public void ResetBurnedInfoByCharacter(int characterID)
    {
        var burned = _stats.BurnedInfo.FirstOrDefault(i => i.CharacterID == characterID);
        if (burned == null) return;
        _stats.BurnedInfo.Remove(burned);
        HistoryReset.BurnedInfo.Add(burned);
    }

    public void ResetBurnedInfoBySkill(int skillID) 
    {
        var burned = _stats.BurnedInfo.FirstOrDefault(i => i.SkillID == skillID);
        if (burned == null) return;
        _stats.BurnedInfo.Remove(burned);
        HistoryReset.BurnedInfo.Add(burned);
    }
    
    public void ResetBurnedInfoAll()
    {
        foreach (var burned in _stats.BurnedInfo.ToImmutableHashSet())
        {
            _stats.BurnedInfo.Remove(burned);
            HistoryReset.BurnedInfo.Add(burned);
        }
    }
}
