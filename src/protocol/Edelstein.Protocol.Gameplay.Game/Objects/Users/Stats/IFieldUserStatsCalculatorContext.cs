using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat.Stats;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

public interface IFieldUserStatsCalculatorContext
{
    IFieldUser User { get; }
    
    WeaponType Weapon { get; }
    WeaponType WeaponSub { get; }
    
    IStatModifier STR { get; }
    IStatModifier DEX { get; }
    IStatModifier INT { get; }
    IStatModifier LUK { get; }
    
    IStatModifier MaxHP { get; }
    IStatModifier MaxMP { get; }
    
    IStatModifier PAD { get; }
    IStatModifier PDD { get; }
    IStatModifier MAD { get; }
    IStatModifier MDD { get; }
    IStatModifier ACC { get; }
    IStatModifier EVA { get; }
    
    IStatModifier Craft { get; }
    IStatModifier Speed { get; }
    IStatModifier Jump { get; }
    
    IStatModifier Mastery { get; }
    
    IStatModifier DamageMin { get; }
    IStatModifier DamageMax { get; }
}
