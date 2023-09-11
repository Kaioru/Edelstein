namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

public interface ITwoStatePartyBoosterRecord : ITwoStateTemporaryStatRecord
{
    DateTime DateStart { get; set; }
    TimeSpan Term { get; set; }
    
    bool IsExpired(DateTime now);
}
