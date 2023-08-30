using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IDamageCalculator
{
    uint InitSeed1 { get; }
    uint InitSeed2 { get; }
    uint InitSeed3 { get; }
    
    Task<IUserDamage[]> CalculatePDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack);
    Task<IUserDamage[]> CalculateMDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack);
}
