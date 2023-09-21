namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendDeleteRequest(
    int CharacterID,
    int FriendID
);
