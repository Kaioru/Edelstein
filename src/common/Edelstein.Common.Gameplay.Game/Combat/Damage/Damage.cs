using Edelstein.Protocol.Gameplay.Game.Combat.Damage;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public record struct Damage(
    int Value, 
    bool IsCritical = false
) : IDamage;
