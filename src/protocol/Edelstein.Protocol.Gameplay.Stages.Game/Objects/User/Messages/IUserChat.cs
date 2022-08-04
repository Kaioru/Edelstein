namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;

public interface IUserChat : IFieldUserMessage
{
    string Message { get; }
    bool isOnlyBalloon { get; }
}
