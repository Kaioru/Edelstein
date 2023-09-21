namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyCreateRequest(
    int CharacterID,
    string CharacterName,
    int Job,
    int Level,
    int ChannelID,
    int FieldID
);
