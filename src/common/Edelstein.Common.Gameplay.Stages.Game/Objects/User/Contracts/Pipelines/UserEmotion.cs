using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public record UserEmotion(
    IFieldUser User,
    int Emotion,
    int Duration,
    bool ByItemOption
) : IUserEmotion;
