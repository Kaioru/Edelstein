namespace Edelstein.Protocol.Gameplay.Stages;

public interface IStage<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    IReadOnlyCollection<TStageUser> Users { get; }

    Task Enter(TStageUser user);
    Task Leave(TStageUser user);
}
