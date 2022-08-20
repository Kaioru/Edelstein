namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

public interface IUserLeaveStage<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
