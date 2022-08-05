namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnDisconnect<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
