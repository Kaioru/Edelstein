using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;

public record TwoStateTemporaryStatRecordDynamicTerm : TwoStateTemporaryStatRecord, ITwoStateTemporaryStatRecordDynamicTerm
{
    public TimeSpan Term { get; set; }
    
    public virtual bool IsExpired(DateTime now) => now > DateUpdated.Add(Term);
}
