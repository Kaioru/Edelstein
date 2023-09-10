using Edelstein.Protocol.Gameplay.Game.Combat.Damage;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public record UserAttack(
    int SkillID, 
    int SkillLevel, 
    int Keydown, 
    int AttackAction,
    bool IsFinalAfterSlashBlast, 
    bool IsSoulArrow, 
    bool IsShadowPartner, 
    bool IsSerialAttack, 
    bool IsSpiritJavelin, 
    bool IsSpark
) : IUserAttack;
