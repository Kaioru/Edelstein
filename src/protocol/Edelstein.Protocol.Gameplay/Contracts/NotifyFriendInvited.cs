using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyFriendInvited(
    int InviterID,
    string InviterName,
    int InviterLevel,
    int InviterJob,
    int FriendID,
    IFriend Friend
);
