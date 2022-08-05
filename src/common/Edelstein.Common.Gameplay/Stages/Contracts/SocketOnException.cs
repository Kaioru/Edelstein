using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Contracts;

public record SocketOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) : ISocketOnException<TStageUser> where TStageUser : IStageUser<TStageUser>;
