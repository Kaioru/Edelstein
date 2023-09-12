namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IMobBurnedInfo
{
    int CharacterID { get; }
    int SkillID { get; }
    
    int Damage { get; }
    
    TimeSpan Interval { get; }
    DateTime DateStart { get; }
    DateTime DateExpire { get; }
}
