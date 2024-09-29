using Edelstein.Common.Gameplay.Game.Combat.Stats;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public record FieldUserStatsCalculatorContext(
    IFieldUser User,
    WeaponType Weapon,
    WeaponType WeaponSub
) : IFieldUserStatsCalculatorContext
{
    public IStatModifier STR { get; } = new StatModifier();
    public IStatModifier DEX { get; } = new StatModifier();
    public IStatModifier INT { get; } = new StatModifier();
    public IStatModifier LUK { get; } = new StatModifier();
    
    public IStatModifier MaxHP { get; } = new StatModifier(Max: 99999);
    public IStatModifier MaxMP { get; } = new StatModifier(Max: 99999);
    
    public IStatModifier PAD { get; } = new StatModifier(Max: 29999);
    public IStatModifier PDD { get; } = new StatModifier(Max: 30000);
    public IStatModifier MAD { get; } = new StatModifier(Max: 29999);
    public IStatModifier MDD { get; } = new StatModifier(Max: 30000);
    public IStatModifier ACC { get; } = new StatModifier();
    public IStatModifier EVA { get; } = new StatModifier();
    
    public IStatModifier Craft { get; } = new StatModifier();
    public IStatModifier Speed { get; } = new StatModifier(100, 140);
    public IStatModifier Jump { get; } = new StatModifier(100, 123);

    public IStatModifier Mastery { get; } = new StatModifier();
    
    public IStatModifier DamageMin { get; } = new StatModifier(1, 999999);
    public IStatModifier DamageMax { get; } = new StatModifier(1, 999999);
}
