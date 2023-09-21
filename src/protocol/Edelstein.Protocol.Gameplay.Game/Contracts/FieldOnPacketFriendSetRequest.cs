using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketFriendSetRequest(
    IFieldUser User,
    string FriendName,
    string FriendGroup
);
