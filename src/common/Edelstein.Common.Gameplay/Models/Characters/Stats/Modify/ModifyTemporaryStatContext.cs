using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;

public class ModifyTemporaryStatContext : IModifyTemporaryStatContext
{
    private readonly ICharacterTemporaryStats _stats;
    
    public ModifyTemporaryStatContext(ICharacterTemporaryStats stats)
    {
        _stats = stats;
        HistoryReset = new CharacterTemporaryStats();
        HistorySet = new CharacterTemporaryStats();
    }
        
    public ICharacterTemporaryStats HistoryReset { get; }
    public ICharacterTemporaryStats HistorySet { get; }

    public void Set(TemporaryStatType type, ITemporaryStatRecord stat)
    {
        ResetByType(type);

        _stats.Records[type] = stat;
        HistorySet.Records[type] = stat;
    }

    public void Set(TemporaryStatType type, int value, int reason, DateTime? dateExpire = null)
        => Set(type, new TemporaryStatRecord
        {
            Value = value,
            Reason = reason,
            DateExpire = dateExpire
        });
    
    public void ResetByType(TemporaryStatType type)
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
