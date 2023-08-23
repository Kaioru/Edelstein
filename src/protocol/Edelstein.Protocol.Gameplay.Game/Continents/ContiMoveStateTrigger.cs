namespace Edelstein.Protocol.Gameplay.Game.Continents;

public enum ContiMoveStateTrigger : byte
{
    Board = 0x1,
    Start = 0x2,
    MobGen = 0x4,
    MobDestroy = 0x5,
    End = 0x6
}
