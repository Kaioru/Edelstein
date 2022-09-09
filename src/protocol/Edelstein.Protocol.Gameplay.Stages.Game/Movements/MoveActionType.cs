namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements;

public enum MoveActionType : byte
{
    Walk = 0x1,
    Move = 0x1,
    Stand = 0x2,
    Jump = 0x3,
    Alert = 0x4,
    Prone = 0x5,
    Fly1 = 0x6,
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
    Fly2 = 0x11,
    Fly2_Move = 0x12,
    Dash2 = 0x13,
    RocketBooster = 0x14,
    TeslaCoilTriangle = 0x15,
    BackWalk = 0x16,
    BladeStance = 0x17,
    FeverMode = 0x18
}
