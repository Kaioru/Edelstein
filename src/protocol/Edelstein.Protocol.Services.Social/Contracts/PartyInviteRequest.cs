namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyInviteRequest(
    int InviterID,
    string InviterName,
    int InviterLevel,
    int InviterJob,
    int PartyID,
    string CharacterName
);
