using Edelstein.Protocol.Gameplay.Game.Combat.Stats;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IFieldMobStatsCalculatorContext
{
    IFieldMob Mob { get; }
    
    IStatModifier PAD { get; }
    IStatModifier PDD { get; }
    IStatModifier PDR { get; }
    IStatModifier MAD { get; }
    IStatModifier MDD { get; }
    IStatModifier MDR { get; }
    IStatModifier ACC { get; }
    IStatModifier EVA { get; }
}
