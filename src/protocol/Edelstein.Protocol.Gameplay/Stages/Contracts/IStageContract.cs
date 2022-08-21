namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface IStageContract<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    IStage<TStageUser> Stage { get; }
}
