using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnMigrateOut<TStageUser>(
    TStageUser User,
    string ServerID
) : ISocketOnMigrateOut<TStageUser> where TStageUser : IStageUser<TStageUser>;
