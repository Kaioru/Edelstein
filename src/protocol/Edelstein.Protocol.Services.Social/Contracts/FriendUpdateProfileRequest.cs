namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendUpdateProfileRequest(
    int CharacterID,
    byte FriendMax,
    bool IsMaster
);
