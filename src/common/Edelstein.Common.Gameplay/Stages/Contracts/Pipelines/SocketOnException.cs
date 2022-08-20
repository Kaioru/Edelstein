using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) : ISocketOnException<TStageUser> where TStageUser : IStageUser<TStageUser>;
