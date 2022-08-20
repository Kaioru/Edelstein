namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface IStageUserContract<out TStageUser> where TStageUser : IStageUser<TStageUser>
{
    TStageUser User { get; }
}
