namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyInviteRejectRequest(
    int CharacterID,
    string CharacterName,
    int PartyID
);
