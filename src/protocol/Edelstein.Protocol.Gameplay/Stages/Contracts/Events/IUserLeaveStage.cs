namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

public interface IUserLeaveStage<TStageUser> :
    IStageUserContract<TStageUser>,
    IStageContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
