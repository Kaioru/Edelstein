namespace Edelstein.Protocol.Services.Social.Contracts;

public enum PartyResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyInParty = 0x3,
    FailedNotBoss = 0x4
}
