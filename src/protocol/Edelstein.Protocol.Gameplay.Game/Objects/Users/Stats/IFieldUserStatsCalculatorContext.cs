using Edelstein.Protocol.Gameplay.Game.Combat.Stats;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

public interface IFieldUserStatsCalculatorContext
{
    IFieldUser User { get; }
    
    IStatModifier STR { get; }
    IStatModifier DEX { get; }
    IStatModifier INT { get; }
    IStatModifier LUK { get; }
}
