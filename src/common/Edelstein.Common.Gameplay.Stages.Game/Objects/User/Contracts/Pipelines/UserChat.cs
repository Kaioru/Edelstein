using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public record UserChat(
    IFieldUser User,
    string Message,
    bool isOnlyBalloon
) : IUserChat;
