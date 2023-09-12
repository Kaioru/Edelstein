using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface ISummonedAttack
{
    byte MobCount { get; }
    
    byte AttackActionAndDir { get; }
    byte AttackAction => (byte)(AttackActionAndDir & 0x7F);
    bool AttackIsLeft => (AttackActionAndDir >> 7 & 1) > 0;
    
    IPoint2D PositionOwner { get; }
    IPoint2D PositionSummoned { get; }
    
    int RepeatSkillPoint { get; }
    
    IAttackMobEntry[] MobEntries { get; }
}
