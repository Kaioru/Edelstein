namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface IStageUserMessage<out TStageUser> where TStageUser : IStageUser<TStageUser>
{
    TStageUser User { get; }
}
