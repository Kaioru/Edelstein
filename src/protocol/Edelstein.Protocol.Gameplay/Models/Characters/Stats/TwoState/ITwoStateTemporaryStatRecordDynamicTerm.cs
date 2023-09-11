namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

public interface ITwoStateTemporaryStatRecordDynamicTerm : ITwoStateTemporaryStatRecord
{
    TimeSpan Term { get; set; }
    
    bool IsExpired(DateTime now);
}
