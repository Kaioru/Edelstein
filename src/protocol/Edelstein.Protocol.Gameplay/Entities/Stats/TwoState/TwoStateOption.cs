using System;

namespace Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

public record TwoStateOption : TemporaryStatOption
{
    public DateTime DateUpdated { get; set; }
}
