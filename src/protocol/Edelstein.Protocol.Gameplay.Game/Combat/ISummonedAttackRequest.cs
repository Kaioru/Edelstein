using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISummonedAttackRequest
{
    int MobCount { get; }
    
    bool IsLeft { get; }
    
    int AttackAction { get; }
    
    IPoint2D OwnerPosition { get; }
    IPoint2D SummonedPosition { get; }
    
    int RepeatSkillPoint { get; }
    
    ICollection<IAttackRequestEntry> Entries { get; }
}
