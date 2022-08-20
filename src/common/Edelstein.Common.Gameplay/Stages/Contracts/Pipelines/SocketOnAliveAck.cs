using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnAliveAck<TStageUser>(
    TStageUser User,
    DateTime Date
) : ISocketOnAliveAck<TStageUser> where TStageUser : IStageUser<TStageUser>;
