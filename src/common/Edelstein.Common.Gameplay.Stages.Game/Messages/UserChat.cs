using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Messages;

public record UserChat(
    IFieldUser User,
    string Message,
    bool isOnlyBalloon
) : IUserChat;
