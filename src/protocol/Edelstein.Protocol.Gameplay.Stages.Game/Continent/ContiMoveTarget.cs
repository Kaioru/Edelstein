namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public class ContiMoveTarget : byte
    {
        TargetStartField = 0x7,
        TargetStartShipMoveField = 0x8,
        TargetWaitField = 0x9,
        TargetMoveField = 0xA,
        TargetEndField = 0xB,
        TargetEndShipMoveField = 0xC
    }
}
