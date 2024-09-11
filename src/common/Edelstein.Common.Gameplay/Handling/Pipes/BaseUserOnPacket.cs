using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Pipes;

public class BaseUserOnPacket<TStageSystem, TStageSystemUser>(
    IPacketHandlerManager<TStageSystem, TStageSystemUser> manager
) : IPipe<UserOnPacket<TStageSystem, TStageSystemUser>>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public virtual Task Handle(IPipelineContext ctx, UserOnPacket<TStageSystem, TStageSystemUser> message)
        => manager.Process(message.User, message.Packet);
}
