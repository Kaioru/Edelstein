namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnMigrateOut<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    string ServerID { get; }
}
