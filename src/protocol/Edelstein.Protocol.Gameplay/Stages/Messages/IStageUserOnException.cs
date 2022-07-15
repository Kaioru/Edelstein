namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface IStageUserOnException<out TStageUser> : IStageUserRequest<TStageUser> where TStageUser : IStageUser
{
    Exception Exception { get; }
}
