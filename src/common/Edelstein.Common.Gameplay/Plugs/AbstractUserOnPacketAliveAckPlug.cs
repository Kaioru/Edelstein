using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Plugs;

public class AbstractUserOnPacketAliveAckPlug<TStageUser> : IPipelinePlug<UserOnPacketAliveAck<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public virtual Task Handle(IPipelineContext ctx, UserOnPacketAliveAck<TStageUser> message)
    {
        message.User.Socket.LastAliveRecv = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
