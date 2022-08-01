namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnMigrateOut<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    string ServerID { get; }
}
