namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface IStageUserOnDisconnect<out TStageUser> : IStageUserRequest<TStageUser> where TStageUser : IStageUser
{
}
