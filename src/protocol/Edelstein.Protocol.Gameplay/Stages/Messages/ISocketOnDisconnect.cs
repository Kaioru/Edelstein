namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface ISocketOnDisconnect<out TStageUser> : IStageUserMessage<TStageUser> where TStageUser : IStageUser
{
}
