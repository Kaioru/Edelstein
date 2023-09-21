namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyUpdateChannelOrFieldRequest(
    int PartyID,
    int CharacterID,
    int ChannelID,
    int FieldID
);
