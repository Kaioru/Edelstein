namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserChat : IFieldUserContract
{
    string Message { get; }
    bool isOnlyBalloon { get; }
}
