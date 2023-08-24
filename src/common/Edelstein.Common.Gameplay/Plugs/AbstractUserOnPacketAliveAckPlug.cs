using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Plugs;

public abstract class AbstractUserOnPacketAliveAckPlug<TStageUser> : IPipelinePlug<UserOnPacketAliveAck<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public Task Handle(IPipelineContext ctx, UserOnPacketAliveAck<TStageUser> message)
    {
        message.User.Socket.LastAliveRecv = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
