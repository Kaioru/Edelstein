using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IDamageCalculator
{
    Task CalculatePDamage(IFieldUserStats attacker, IFieldMobStats target);
    Task CalculateMDamage(IFieldUserStats attacker, IFieldMobStats target);
}
