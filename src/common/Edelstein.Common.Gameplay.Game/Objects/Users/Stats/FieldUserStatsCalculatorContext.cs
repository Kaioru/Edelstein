using Edelstein.Common.Gameplay.Game.Combat.Stats;
using Edelstein.Protocol.Gameplay.Game.Combat.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public record FieldUserStatsCalculatorContext(
    IFieldUser User
) : IFieldUserStatsCalculatorContext
{
    public IStatModifier STR { get; } = new StatModifier();
    public IStatModifier DEX { get; } = new StatModifier();
    public IStatModifier INT { get; } = new StatModifier();
    public IStatModifier LUK { get; } = new StatModifier();
    
    public IStatModifier MaxHP { get; } = new StatModifier(max: 99999);
    public IStatModifier MaxMP { get; } = new StatModifier(max: 99999);
    
    public IStatModifier PAD { get; } = new StatModifier(max: 29999);
    public IStatModifier PDD { get; } = new StatModifier(max: 30000);
    public IStatModifier MAD { get; } = new StatModifier(max: 29999);
    public IStatModifier MDD { get; } = new StatModifier(max: 30000);
    public IStatModifier ACC { get; } = new StatModifier(max: 9999);
    public IStatModifier EVA { get; } = new StatModifier(max: 9999);
    
    public IStatModifier Craft { get; } = new StatModifier();
    public IStatModifier Speed { get; } = new StatModifier(100, 140);
    public IStatModifier Jump { get; } = new StatModifier(100, 123);
}
