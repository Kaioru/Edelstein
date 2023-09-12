using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;

public record TwoStatePartyBoosterRecord : TwoStateTemporaryStatRecord, ITwoStatePartyBoosterRecord
{
    public DateTime DateStart { get; set; }
    public TimeSpan Term { get; set; }
    
    public virtual bool IsExpired(DateTime now) => now > DateStart.Add(Term);
}
