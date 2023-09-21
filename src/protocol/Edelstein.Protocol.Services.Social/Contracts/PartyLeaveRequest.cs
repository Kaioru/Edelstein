namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyLeaveRequest(
    int CharacterID,
    string CharacterName,
    int PartyID
);
