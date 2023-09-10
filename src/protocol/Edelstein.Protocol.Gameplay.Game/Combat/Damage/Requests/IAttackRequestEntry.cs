using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage.Requests;

public interface IAttackRequestEntry
{
    int MobID { get; }
    
    byte HitAction { get; }
    byte ForeAction { get; }
    bool IsLeft { get; }
    
    byte FrameIDx { get; }
    
    byte CalcDamageStatIndex { get; }
    bool IsDoomed { get; }
    
    IPoint2D HitPosition { get; }
    IPoint2D PrevPosition { get; }
    
    short Delay { get; }
    
    int[] Damage { get; }
}
