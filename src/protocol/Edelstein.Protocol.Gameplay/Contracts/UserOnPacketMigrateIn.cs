namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacketMigrateIn<TStageUser>(
    TStageUser User,
    int CharacterID,
    long Key
) where TStageUser : IStageUser<TStageUser>;
