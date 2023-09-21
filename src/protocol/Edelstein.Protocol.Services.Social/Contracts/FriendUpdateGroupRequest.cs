namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendUpdateGroupRequest(
    int CharacterID,
    int FriendID,
    string FriendGroup
);
