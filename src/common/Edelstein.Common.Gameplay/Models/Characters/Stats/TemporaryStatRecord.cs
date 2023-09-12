using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats;

public record TemporaryStatRecord : ITemporaryStatRecord
{
    public int Value { get; set; }
    public int Reason { get; set; }
    
    public DateTime? DateExpire { get; set; }
}
