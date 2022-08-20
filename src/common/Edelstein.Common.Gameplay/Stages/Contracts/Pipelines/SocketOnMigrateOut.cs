using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnMigrateOut<TStageUser>(
    TStageUser User,
    string ServerID
) : ISocketOnMigrateOut<TStageUser> where TStageUser : IStageUser<TStageUser>;
