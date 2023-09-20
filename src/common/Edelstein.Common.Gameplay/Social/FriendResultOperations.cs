namespace Edelstein.Common.Gameplay.Social;

public enum FriendResultOperations
{
    LoadFriend_Done = 0x7,
    NotifyChange_FriendInfo = 0x8,
    Invite = 0x9,
    SetFriend_Done = 0xA,

    SetFriendFullMe = 0xB,
    SetFriendFullOther = 0xC,
    SetFriendAlreadySet = 0xD,
    SetFriendMaster = 0xE,
    SetFriendUnknownUser = 0xF,

    SetFriendUnknown = 0x10,
    AcceptFriendUnknown = 0x11,
    DeleteFriendUnknown = 0x13,
    IncMaxCountUnknown = 0x16,

    DeleteFriendDone = 0x12,
    Notify = 0x14,
    IncMaxCountDone = 0x15,
    PleaseWait = 0x17
}
