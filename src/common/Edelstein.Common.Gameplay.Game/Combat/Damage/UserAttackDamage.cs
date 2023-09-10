using Edelstein.Protocol.Gameplay.Game.Combat.Damage;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public record UserAttackDamage(
    int Damage, 
    bool IsCritical = false
) : IUserAttackDamage;
