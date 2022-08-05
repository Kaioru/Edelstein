namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserChat : IFieldUserContract
{
    string Message { get; }
    bool isOnlyBalloon { get; }
}
