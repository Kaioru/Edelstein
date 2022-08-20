namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

public interface ISocketOnMigrateOut<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    string ServerID { get; }
}
