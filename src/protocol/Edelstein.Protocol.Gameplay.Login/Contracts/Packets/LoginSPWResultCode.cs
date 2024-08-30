namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets;

public enum LoginSPWResultCode : byte
{
    Success = 0x0,
    DBFail = 0x6,
    Unknown = 0x9,
    IncorrectSPW = 0x14,
    SamePasswordAndSPW = 0x16,
    SamePincodeAndSPW = 0x17,
}
