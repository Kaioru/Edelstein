namespace Edelstein.Service.Game.Fields.Movements
{
    public enum MoveActionType : byte
    {
        Walk = 0x1,
        Move = 0x1,
        Stand = 0x2,
        Jump = 0x3,
        Alert = 0x4,
        Prone = 0x5,
        Fly = 0x6,
        Ladder = 0x7,
        Rope = 0x8,
        Dead = 0x9,
        Sit = 0xA,
        Stand0 = 0xB,
        Hungry = 0xC,
        Rest0 = 0xD,
        Rest1 = 0xE,
        Hang = 0xF,
        Chase = 0x10,
        No = 0x11
    }
}