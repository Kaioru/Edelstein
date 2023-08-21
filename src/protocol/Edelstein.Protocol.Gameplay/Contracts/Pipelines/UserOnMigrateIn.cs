namespace Edelstein.Protocol.Gameplay.Contracts.Pipelines;

public record UserOnMigrateIn<TStageUser>(
    TStageUser User,
    int CharacterID,
    long Key
) where TStageUser : IStageUser<TStageUser>;
