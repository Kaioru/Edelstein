namespace Edelstein.Common.Gameplay.Social;

public enum FriendRequestOperations
{
    LoadFriend = 0x0,

    SetFriend = 0x1,
    AcceptFriend = 0x2,
    DeleteFriend = 0x3,

    NotifyLogin = 0x4,
    NotifyLogout = 0x5,
    IncMaxCount = 0x6
}
