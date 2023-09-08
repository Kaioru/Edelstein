namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IUserAttack
{
    int SkillID { get; }
    int SkillLevel { get; }
    
    int Keydown { get; }
}
