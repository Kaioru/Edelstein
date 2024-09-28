using System;

namespace Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

public record TwoStateDynamicTermOption : TwoStateOption
{
    public TimeSpan Term { get; set; }
}
