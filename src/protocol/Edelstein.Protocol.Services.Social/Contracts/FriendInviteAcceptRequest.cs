namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendInviteAcceptRequest(
    int InviterID,
    int FriendID,
    int ChannelID
);
