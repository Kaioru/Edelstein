using Edelstein.Protocol.Gameplay.Stages.Chat;
using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Chat.Contexts;

public record ChatContextPipelines(
    IPipeline<ISocketOnMigrateIn<IChatStageUser>> SocketOnMigrateIn,
    IPipeline<ISocketOnMigrateOut<IChatStageUser>> SocketOnMigrateOut,
    IPipeline<ISocketOnAliveAck<IChatStageUser>> SocketOnAliveAck,
    IPipeline<ISocketOnPacket<IChatStageUser>> SocketOnPacket,
    IPipeline<ISocketOnException<IChatStageUser>> SocketOnException,
    IPipeline<ISocketOnDisconnect<IChatStageUser>> SocketOnDisconnect
) : IChatContextPipelines;
