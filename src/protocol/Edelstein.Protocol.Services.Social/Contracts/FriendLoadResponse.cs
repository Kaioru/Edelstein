namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendLoadResponse(
    FriendResult Result,
    IFriendList Friends
);
