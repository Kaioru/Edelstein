using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Actions;

public abstract class AbstractSocketOnDisconnectPlug<TStageUser> : IPipelinePlug<ISocketOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly ISessionService _session;

    protected AbstractSocketOnDisconnectPlug(ISessionService session) => _session = session;

    public async Task Handle(IPipelineContext ctx, ISocketOnDisconnect<TStageUser> message)
    {
        if (message.User.Stage != null)
            await message.User.Stage.Leave(message.User);
        if (message.User.Account != null)
            await _session.End(new SessionEndRequest(message.User.Account.ID));
    }
}
