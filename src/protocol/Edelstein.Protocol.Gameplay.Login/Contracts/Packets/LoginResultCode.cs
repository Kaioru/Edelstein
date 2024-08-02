namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets;

public enum LoginResultCode : byte
{
    ProcFail = byte.MaxValue,
    Success = 0x0,
    TempBlocked = 0x1,
    Blocked = 0x2,
    Abandoned = 0x3,
    IncorrectPassword = 0x4,
    NotRegistered = 0x5,
    DBFail = 0x6,
    AlreadyConnected = 0x7,
    NotConnectableWorld = 0x8,
    Unknown = 0x9,
    Timeout = 0xA,
    NotAdult = 0xB,
    AuthFail = 0xC,
    ImpossibleIP = 0xD,
    NotAuthorizedNexonID = 0xE,
    NoNexonID = 0xF,
    NotAuthorized = 0x10,
    InvalidRegionInfo = 0x11,
    InvalidBirthDate = 0x12,
    PassportSuspended = 0x13,
    IncorrectSSN2 = 0x14,
    WebAuthNeeded = 0x15,
    DeleteCharacterFailedOnGuildMaster = 0x16,
    NotAgreedEULA = 0x17,
    DeleteCharacterFailedEngaged = 0x18
}
