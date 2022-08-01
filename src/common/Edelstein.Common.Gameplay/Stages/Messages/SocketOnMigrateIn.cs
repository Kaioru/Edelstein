using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnMigrateIn<TStageUser>(
    TStageUser User,
    int CharacterID,
    long Key
) : ISocketOnMigrateIn<TStageUser> where TStageUser : IStageUser<TStageUser>;
