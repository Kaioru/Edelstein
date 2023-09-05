using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IDamageCalculator
{
    uint InitSeed1 { get; }
    uint InitSeed2 { get; }
    uint InitSeed3 { get; }

    void Skip();
    
    Task<IUserAttackDamage[]> CalculatePDamage(ICharacter character, IFieldUserStats stats, IFieldMobStats target, IUserAttack attack);
    Task<IUserAttackDamage[]> CalculateMDamage(ICharacter character, IFieldUserStats stats, IFieldMobStats target, IUserAttack attack);
}
