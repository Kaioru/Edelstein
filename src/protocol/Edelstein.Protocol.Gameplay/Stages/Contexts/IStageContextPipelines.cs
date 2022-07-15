using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Contexts;

public interface IStageContextPipelines<TStageUser> where TStageUser : IStageUser
{
    IPipeline<IStageUserOnPacket<TStageUser>> StageUserOnPacket { get; }
    IPipeline<IStageUserOnException<TStageUser>> StageUserOnException { get; }
    IPipeline<IStageUserOnDisconnect<TStageUser>> StageUserOnDisconnect { get; }
}
