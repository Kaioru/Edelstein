using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<ISocketOnMigrateIn<ILoginStageUser>> SocketOnMigrateIn,
    IPipeline<ISocketOnMigrateOut<ILoginStageUser>> SocketOnMigrateOut,
    IPipeline<ISocketOnAliveAck<ILoginStageUser>> SocketOnAliveAck,
    IPipeline<ISocketOnPacket<ILoginStageUser>> SocketOnPacket,
    IPipeline<ISocketOnException<ILoginStageUser>> SocketOnException,
    IPipeline<ISocketOnDisconnect<ILoginStageUser>> SocketOnDisconnect
) : ILoginContextPipelines;
