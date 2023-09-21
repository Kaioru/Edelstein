using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketPartyChangeLeaderRequest(
    IFieldUser User,
    int CharacterID
);
