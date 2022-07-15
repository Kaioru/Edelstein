using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Contexts;

public interface IStageContextPipelines<TStageUser> where TStageUser : IStageUser
{
    IPipeline<ISocketOnPacket<TStageUser>> SocketOnPacket { get; }
    IPipeline<ISocketOnException<TStageUser>> SocketOnException { get; }
    IPipeline<ISocketOnDisconnect<TStageUser>> SocketOnDisconnect { get; }
}
