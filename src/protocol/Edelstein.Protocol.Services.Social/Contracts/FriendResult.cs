namespace Edelstein.Protocol.Services.Social.Contracts;

public enum FriendResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedCharacterNotFound = 0x3,
    FailedAlreadyAdded = 0x4,
    FailedMaxSlotMe = 0x5,
    FailedMaxSlotOther = 0x6,
    FailedNotInvited = 0x7,
    FailedMaster = 0x8,
    FailedSelf = 0x9
}
