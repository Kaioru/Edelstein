namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyInviteAcceptRequest(
    int CharacterID,
    string CharacterName,
    int Job,
    int Level,
    int ChannelID,
    int FieldID,
    int PartyID
);
