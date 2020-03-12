namespace Edelstein.Service.Game.Fields.Continent
{
    public enum ContinentTarget : byte
    {
        TargetStartField = 0x7,
        TargetStartShipMoveField = 0x8,
        TargetWaitField = 0x9,
        TargetMoveField = 0xA,
        TargetEndField = 0xB,
        TargetEndShipMoveField = 0xC
    }
}