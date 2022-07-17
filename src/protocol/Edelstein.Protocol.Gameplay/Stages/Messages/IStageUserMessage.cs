namespace Edelstein.Protocol.Gameplay.Stages.Messages;

public interface IStageUserMessage<out TStageUser> where TStageUser : IStageUser<TStageUser>
{
    TStageUser User { get; }
}
