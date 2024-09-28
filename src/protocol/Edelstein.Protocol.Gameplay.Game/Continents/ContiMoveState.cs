namespace Edelstein.Protocol.Gameplay.Game.Continents;

public enum ContiMoveState : byte
{
    Dormant = 0x0,
    Wait = 0x1,
    Move = 0x3,
    Event = 0x4
}
