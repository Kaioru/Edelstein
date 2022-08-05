namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnException<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    Exception Exception { get; }
}
