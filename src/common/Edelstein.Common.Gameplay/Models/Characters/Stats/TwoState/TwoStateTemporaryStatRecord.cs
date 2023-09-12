using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;

public record TwoStateTemporaryStatRecord : ITwoStateTemporaryStatRecord
{
    public int Value { get; set; }
    public int Reason { get; set; }
    
    public DateTime DateUpdated { get; set; }

    public virtual bool IsActive() => Value != 0;
}
