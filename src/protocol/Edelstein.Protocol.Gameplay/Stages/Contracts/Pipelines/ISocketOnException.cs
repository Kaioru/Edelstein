namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

public interface ISocketOnException<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    Exception Exception { get; }
}
