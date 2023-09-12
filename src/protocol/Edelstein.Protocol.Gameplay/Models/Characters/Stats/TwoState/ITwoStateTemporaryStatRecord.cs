namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

public interface ITwoStateTemporaryStatRecord
{
    int Value { get; set; }
    int Reason { get; set; }
    
    DateTime DateUpdated { get; set; }

    bool IsActive();
}
