using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnAliveAck<TStageUser>(
    TStageUser User,
    DateTime Date
) : ISocketOnAliveAck<TStageUser> where TStageUser : IStageUser<TStageUser>;
