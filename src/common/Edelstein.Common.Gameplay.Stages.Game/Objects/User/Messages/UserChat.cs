using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public record UserChat(
    IFieldUser User,
    string Message,
    bool isOnlyBalloon
) : IUserChat;
