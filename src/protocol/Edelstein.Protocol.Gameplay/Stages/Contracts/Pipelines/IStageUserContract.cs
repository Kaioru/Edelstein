namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

public interface IStageUserContract<out TStageUser> where TStageUser : IStageUser<TStageUser>
{
    TStageUser User { get; }
}
