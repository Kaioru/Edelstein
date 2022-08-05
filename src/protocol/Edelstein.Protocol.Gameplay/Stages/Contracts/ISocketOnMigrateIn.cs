namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnMigrateIn<out TStageUser> : IStageUserMessage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    int CharacterID { get; }
    long Key { get; }
}
