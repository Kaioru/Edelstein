using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Actions;

public abstract class AbstractSocketOnDisconnectPlug<TStageUser> : IPipelinePlug<ISocketOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public async Task Handle(IPipelineContext ctx, ISocketOnDisconnect<TStageUser> message)
    {
        if (message.User.Stage != null)
            await message.User.Stage.Leave(message.User);
    }
}
