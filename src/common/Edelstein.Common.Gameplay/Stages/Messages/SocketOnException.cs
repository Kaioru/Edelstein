using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) : ISocketOnException<TStageUser> where TStageUser : IStageUser<TStageUser>;
