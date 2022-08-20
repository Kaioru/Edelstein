namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

public interface IUserEnterStage<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
