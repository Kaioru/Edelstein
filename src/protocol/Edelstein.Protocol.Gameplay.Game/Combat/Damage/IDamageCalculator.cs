using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface IDamageCalculator
{
    uint InitSeed1 { get; }
    uint InitSeed2 { get; }
    uint InitSeed3 { get; }

    void Skip();
    
    Task<IDamage[]> CalculatePDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IAttack attack,
        IAttackMobEntry attackMob
    );
    Task<IDamage[]> CalculateMDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        IAttack attack,
        IAttackMobEntry attackMob
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

    Task<int[]> CalculateAdjustedDamage(ICharacter character, IFieldUserStats stats, IAttack attack, IDamage[] damage, int count);
}
