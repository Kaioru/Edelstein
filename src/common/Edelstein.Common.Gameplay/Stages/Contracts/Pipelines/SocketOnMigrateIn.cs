using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnMigrateIn<TStageUser>(
    TStageUser User,
    int CharacterID,
    long Key
) : ISocketOnMigrateIn<TStageUser> where TStageUser : IStageUser<TStageUser>;
