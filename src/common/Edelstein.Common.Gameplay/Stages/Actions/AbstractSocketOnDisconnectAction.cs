using Edelstein.Common.Services.Session.Contracts;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Actions;

public abstract class AbstractSocketOnDisconnectAction<TStageUser> : IPipelineAction<ISocketOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly ISessionService _sessions;

    protected AbstractSocketOnDisconnectAction(ISessionService sessions) => _sessions = sessions;

    public async Task Handle(IPipelineContext ctx, ISocketOnDisconnect<TStageUser> message)
    {
        if (message.User.Stage != null)
            await message.User.Stage.Leave(message.User);
        if (message.User.Account != null)
            await _sessions.End(new SessionEndRequest(message.User.Account.ID));
    }
}
