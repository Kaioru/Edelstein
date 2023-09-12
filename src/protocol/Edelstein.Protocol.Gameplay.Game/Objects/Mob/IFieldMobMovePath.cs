using Edelstein.Protocol.Gameplay.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMobMovePath : IMovePath<IFieldMobMoveAction>
{
    short MobCtrlSN { get; }
    
    // v44
    bool NextAttackPossible { get; }
    
    int Action { get; }
    int Data { get; }
    
    // cheatedRandom
    // cheatedCtrlMove
    
    // multiTargetForBall
    // randTimeForAreaAttack
}
