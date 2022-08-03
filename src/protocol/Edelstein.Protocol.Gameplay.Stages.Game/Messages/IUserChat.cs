namespace Edelstein.Protocol.Gameplay.Stages.Game.Messages;

public interface IUserChat : IFieldUserMessage
{
    string Message { get; }
    bool isOnlyBalloon { get; }
}
