using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IDamageCalculator
{
    uint InitSeed1 { get; }
    uint InitSeed2 { get; }
    uint InitSeed3 { get; }

    void Skip();
    
    Task<IUserAttackDamage[]> CalculatePDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IUserAttack attack
    );
    Task<IUserAttackDamage[]> CalculateMDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IUserAttack attack
    );
    
    Task<int> CalculatePDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IFieldSummoned summoned
    );
    
    Task<int> CalculateMDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IFieldSummoned summoned
    );

    Task<int> CalculateBurnedDamage(
        ICharacter character,
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats,
        int skillID,
        int skillLevel
    );

    Task<IUserAttackDamage[]> AdjustDamageDecRate(IFieldUserStats stats, IUserAttack attack, int count, IUserAttackDamage[] damage);
}
