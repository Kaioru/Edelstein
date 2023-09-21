namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyUpdateLevelOrJobRequest(
    int PartyID,
    int CharacterID,
    int Level,
    int Job
);
