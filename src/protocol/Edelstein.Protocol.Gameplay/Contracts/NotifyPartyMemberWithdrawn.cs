namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyMemberWithdrawn(
    int PartyID,
    int CharacterID,
    string CharacterName,
    bool IsKicked
);
