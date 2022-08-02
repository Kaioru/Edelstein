using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnAliveAck<TStageUser>(
    TStageUser User,
    DateTime Date
) : ISocketOnAliveAck<TStageUser> where TStageUser : IStageUser<TStageUser>;
