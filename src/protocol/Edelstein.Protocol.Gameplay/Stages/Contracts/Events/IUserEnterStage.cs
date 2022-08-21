namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

public interface IUserEnterStage<TStageUser> :
    IStageUserContract<TStageUser>,
    IStageContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
}
