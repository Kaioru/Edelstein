namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

public interface ISocketOnMigrateIn<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    int CharacterID { get; }
    long Key { get; }
}
