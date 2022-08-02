namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnAliveAck<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    DateTime Date { get; }
}
