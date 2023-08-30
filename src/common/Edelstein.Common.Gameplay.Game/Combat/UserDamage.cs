using Edelstein.Protocol.Gameplay.Game.Combat;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record UserDamage(
    int Damage, 
    bool IsCritical
) : IUserDamage;
