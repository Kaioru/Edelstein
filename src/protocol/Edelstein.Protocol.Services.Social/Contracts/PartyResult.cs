namespace Edelstein.Protocol.Services.Social.Contracts;

public enum PartyResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyInParty = 0x3,
    FailedNotBoss = 0x4,
    FailedCharacterNotFound = 0x5,
    FailedSelf = 0x6,
    FailedAlreadyInvited = 0x7,
    FailedNotInvited = 0x8,
    FailedFull = 0x9
}
