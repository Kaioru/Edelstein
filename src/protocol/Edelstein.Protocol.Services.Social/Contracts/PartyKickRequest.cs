namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyKickRequest(
    int BossID,
    int PartyID,
    int CharacterID
);
