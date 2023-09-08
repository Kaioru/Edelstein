using Edelstein.Protocol.Gameplay.Game.Combat;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record UserAttack(
    int SkillID, 
    int SkillLevel, 
    int Keydown, 
    bool IsFinalAfterSlashBlast, 
    bool IsSoulArrow, 
    bool IsShadowPartner, 
    bool IsSerialAttack, 
    bool IsSpiritJavelin, 
    bool IsSpark
) : IUserAttack;
