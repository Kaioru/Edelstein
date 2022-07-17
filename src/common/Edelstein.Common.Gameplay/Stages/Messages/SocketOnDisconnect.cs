using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record SocketOnDisconnect<TStageUser>(
    TStageUser User
) : ISocketOnDisconnect<TStageUser> where TStageUser : IStageUser<TStageUser>;
