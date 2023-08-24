using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserEmotion(
    IFieldUser User,
    int Emotion,
    int Duration,
    bool ByItemOption
);
