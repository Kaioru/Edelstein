using System;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public record TemporaryStatOption
{
    public int Option { get; set; }
    public int Reason { get; set; }
    
    public DateTime? DateExpire { get; set; }
}
