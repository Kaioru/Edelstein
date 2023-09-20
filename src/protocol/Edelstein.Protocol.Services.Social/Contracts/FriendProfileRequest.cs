namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendProfileRequest(
    int CharacterID,
    byte FriendMax,
    bool IsMaster
);
