using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IDamageCalculator
{
    uint InitSeed1 { get; }
    uint InitSeed2 { get; }
    uint InitSeed3 { get; }

    void Skip();
    
    Task<IUserAttackDamage[]> CalculatePDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack);
    Task<IUserAttackDamage[]> CalculateMDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack);
}
