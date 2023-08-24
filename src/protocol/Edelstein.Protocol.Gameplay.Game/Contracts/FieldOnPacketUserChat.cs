using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserChat(
    IFieldUser User,
    string Message,
    bool IsOnlyBalloon
);
