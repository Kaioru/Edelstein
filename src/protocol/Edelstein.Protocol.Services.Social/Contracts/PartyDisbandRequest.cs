namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyDisbandRequest(
    int CharacterID,
    int PartyID
);
