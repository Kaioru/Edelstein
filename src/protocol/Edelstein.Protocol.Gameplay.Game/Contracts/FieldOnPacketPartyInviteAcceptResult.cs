using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketPartyInviteAcceptResult(
    IFieldUser User,
    int PartyID
);
