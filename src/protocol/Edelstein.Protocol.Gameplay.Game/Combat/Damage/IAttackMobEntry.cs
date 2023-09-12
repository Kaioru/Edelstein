using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface IAttackMobEntry
{
    int MobID { get; }
    
    byte ActionHit { get; }
    byte ActionForeAndDir { get; }
    short ActionFore => (byte)(ActionForeAndDir & 0x7F);
    bool ActionIsLeft =>  (ActionForeAndDir >> 7 & 1) > 0;
    byte FrameIdx { get; }
    
    byte Option { get; }
    byte CalcDamageStatIndex => (byte)(Option & 0x7F);
    bool IsDoom => (Option >> 7 & 1) > 0;
    
    IPoint2D PositionHit { get; }
    IPoint2D PositionPrev { get; }
    
    short Delay { get; }
    
    int[] Damage { get; }
}
