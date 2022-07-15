namespace Edelstein.Protocol.Gameplay.Stages;

public interface IStage<in TStageUser> where TStageUser : IStageUser
{
    Task Enter(TStageUser user);
    Task Leave(TStageUser user);
}
