using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public abstract class AbstractSocketOnAliveAckPlug<TStageUser> : IPipelinePlug<ISocketOnAliveAck<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public virtual Task Handle(IPipelineContext ctx, ISocketOnAliveAck<TStageUser> message)
    {
        message.User.Socket.LastAliveRecv = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
