namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface IStageUserRequest<out TStageUser> where TStageUser : IStageUser
{
    TStageUser User { get; }
}
