namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserChat : IFieldUserMessage
{
    string Message { get; }
    bool isOnlyBalloon { get; }
}
