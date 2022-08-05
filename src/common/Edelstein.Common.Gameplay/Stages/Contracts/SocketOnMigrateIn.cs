using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Contracts;

public record SocketOnMigrateIn<TStageUser>(
    TStageUser User,
    int CharacterID,
    long Key
) : ISocketOnMigrateIn<TStageUser> where TStageUser : IStageUser<TStageUser>;
