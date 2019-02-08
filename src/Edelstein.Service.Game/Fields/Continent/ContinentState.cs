namespace Edelstein.Service.Game.Fields.Continent
{
    public enum ContinentState : byte
    {
        Dormant = 0x0,
        Wait = 0x1,
        Start = 0x2,
        Move = 0x3,
        MobGen = 0x4,
        MobDestroy = 0x5,
        End = 0x6,
        TargetStartField = 0x7,
        TargetStartShipMoveField = 0x8,
        TargetWaitField = 0x9,
        TargetMoveField = 0xA,
        TargetEndField = 0xB,
        TargetEndShipMoveField = 0xC
    }
}