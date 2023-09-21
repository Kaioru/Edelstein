namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyChangeBossRequest(
    int BossID,
    int PartyID,
    int CharacterID,
    bool IsDisconnected
);
