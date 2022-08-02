using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public abstract class AbstractSocketOnDisconnectPlug<TStageUser> : IPipelinePlug<ISocketOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountWorldRepository _accountWorldRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ISessionService _session;

    protected AbstractSocketOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    )
    {
        _session = session;
        _accountRepository = accountRepository;
        _accountWorldRepository = accountWorldRepository;
        _characterRepository = characterRepository;
    }

    public async Task Handle(IPipelineContext ctx, ISocketOnDisconnect<TStageUser> message)
    {
        if (message.User.Stage != null)
            await message.User.Stage.Leave(message.User);

        if (message.User.Account != null)
        {
            if (!message.User.IsMigrating)
                await _session.End(new SessionEndRequest(message.User.Account.ID));
            await _accountRepository.Update(message.User.Account);
        }

        if (message.User.AccountWorld != null)
            await _accountWorldRepository.Update(message.User.AccountWorld);
        if (message.User.Character != null)
            await _characterRepository.Update(message.User.Character);
    }
}
