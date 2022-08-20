using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Contexts;

public interface IStageContextPipelines<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    IPipeline<ISocketOnMigrateIn<TStageUser>> SocketOnMigrateIn { get; }
    IPipeline<ISocketOnMigrateOut<TStageUser>> SocketOnMigrateOut { get; }
    IPipeline<ISocketOnAliveAck<TStageUser>> SocketOnAliveAck { get; }

    IPipeline<ISocketOnPacket<TStageUser>> SocketOnPacket { get; }
    IPipeline<ISocketOnException<TStageUser>> SocketOnException { get; }
    IPipeline<ISocketOnDisconnect<TStageUser>> SocketOnDisconnect { get; }
}
