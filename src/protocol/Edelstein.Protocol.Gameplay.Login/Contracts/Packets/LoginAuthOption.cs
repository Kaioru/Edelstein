namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets;

public enum LoginAuthOption : byte
{
    NoOption = 0x0,
    NotAllowed = 0x1,
    MaxConnected = 0x2,
    Expired = 0x3,
    WelcomeAddress = 0xB,
    WelcomeTrial = 0xD,
    PrepaidExhausted = 0x13,
    NotAvailableTime = 0x19,
    DifferentIPNotAllowed = 0x1B,
    AccountMachineIDBlocked = 0x1C,
}
