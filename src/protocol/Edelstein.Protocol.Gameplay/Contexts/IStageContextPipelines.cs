using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Contexts;

public interface IStageContextPipelines<TStageUser, TStageSystem>
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    IPipeline<UserOnPacket<TStageUser, TStageSystem>> UserOnPacketPipeline { get; }
    IPipeline<UserOnException<TStageUser, TStageSystem>> UserOnExceptionPipeline { get; }
    IPipeline<UserOnDisconnect<TStageUser, TStageSystem>> UserOnDisconnectPipeline { get; }
}
