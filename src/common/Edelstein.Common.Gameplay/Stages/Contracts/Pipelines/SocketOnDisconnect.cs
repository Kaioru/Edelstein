using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Pipelines;

public record SocketOnDisconnect<TStageUser>(
    TStageUser User
) : ISocketOnDisconnect<TStageUser> where TStageUser : IStageUser<TStageUser>;
