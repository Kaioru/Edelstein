namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnException<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    Exception Exception { get; }
}
