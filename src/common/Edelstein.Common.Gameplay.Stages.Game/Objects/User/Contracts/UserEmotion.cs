using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserEmotion(
    IFieldUser User,
    int Emotion,
    int Duration,
    bool ByItemOption
) : IUserEmotion;
