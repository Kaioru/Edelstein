namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnDisconnect<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
