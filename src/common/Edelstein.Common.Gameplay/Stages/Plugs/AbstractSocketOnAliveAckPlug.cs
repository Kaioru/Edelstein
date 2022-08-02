using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public abstract class AbstractSocketOnAliveAckPlug<TStageUser> : IPipelinePlug<ISocketOnAliveAck<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public Task Handle(IPipelineContext ctx, ISocketOnAliveAck<TStageUser> message)
    {
        message.User.Socket.LastAliveRecv = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
