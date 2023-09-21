using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketFriendDeleteRequest(
    IFieldUser User,
    int FriendID
);
