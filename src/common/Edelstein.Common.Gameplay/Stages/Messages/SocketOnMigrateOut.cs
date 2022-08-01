using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnMigrateOut<TStageUser>(
    TStageUser User,
    string ServerID
) : ISocketOnMigrateOut<TStageUser> where TStageUser : IStageUser<TStageUser>;
