namespace Edelstein.Protocol.Services.Social.Contracts;

public enum FriendResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedCharacterNotFound = 0x3,
    FailedAlreadyAdded = 0x4,
    FailedMaxSlot = 0x5,
    FailedNotInvited = 0x6,
    FailedMaster = 0x7
}
