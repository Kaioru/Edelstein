namespace Edelstein.Protocol.Gameplay;

public interface IStage<TStage, TStageUser>
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
    IEnumerable<TStageUser> GetUsers();
    
    Task Enter(TStageUser user);
    Task Leave(TStageUser user);
}
