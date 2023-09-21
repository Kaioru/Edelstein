namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyMemberInvited(
    int InviterID,
    string InviterName,
    int InviterLevel,
    int InviterJob,
    int PartyID,
    int CharacterID
);
