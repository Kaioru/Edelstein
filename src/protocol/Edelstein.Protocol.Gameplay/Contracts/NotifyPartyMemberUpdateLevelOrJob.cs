namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyMemberUpdateLevelOrJob(
    int PartyID,
    int CharacterID,
    int Level,
    int Job
);
