namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendUpdateChannelRequest(
    int CharacterID,
    int ChannelID
);
