namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyMemberUpdateChannelOrField(
    int PartyID,
    int CharacterID,
    int ChannelID,
    int FieldID
);
