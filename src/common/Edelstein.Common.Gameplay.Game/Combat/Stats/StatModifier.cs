using System;
using Edelstein.Protocol.Gameplay.Game.Combat.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Stats;

public record StatModifier(
    int Min = 0,
    int Max = int.MaxValue
) : IStatModifier
{
    public int Min { get; set; } = Min;
    public int Max { get; set; } = Max;
    
    public int IncBase { get; set; }
    
    public int IncRate { get; set; }
    public int IncFlat { get; set; }

    public int Apply(int value) 
        => Math.Min(Math.Max(value + IncBase + (int)((value + IncBase) * IncRate / 100d) + IncFlat, Min), Max);
}
