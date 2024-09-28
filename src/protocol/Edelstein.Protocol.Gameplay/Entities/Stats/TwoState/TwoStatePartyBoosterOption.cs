using System;

namespace Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

public record TwoStatePartyBoosterOption : TwoStateOption
{
    public DateTime DateStart { get; set; }
    public TimeSpan Term { get; set; }
}
