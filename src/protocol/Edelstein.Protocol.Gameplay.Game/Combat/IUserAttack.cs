namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IUserAttack
{
    int SkillID { get; }
    int SkillLevel { get; }
    
    int Keydown { get; }
    
    bool IsFinalAfterSlashBlast { get; }
    bool IsSoulArrow { get; }
    bool IsShadowPartner { get; }
    bool IsSerialAttack { get; }
    bool IsSpiritJavelin { get; }
    bool IsSpark { get; }
}
