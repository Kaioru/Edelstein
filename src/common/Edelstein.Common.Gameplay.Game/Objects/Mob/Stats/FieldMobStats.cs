using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public record FieldMobStats : IFieldMobStats
{
    public int Level { get; init; }
    
    public int PAD { get; init; }
    public int PDD { get; init; }
    public int PDR { get; init; }
    public int MAD { get; init; }
    public int MDD { get; init; }
    public int MDR { get; init; }
    public int ACC { get; init; }
    public int EVA { get; init; }
}
