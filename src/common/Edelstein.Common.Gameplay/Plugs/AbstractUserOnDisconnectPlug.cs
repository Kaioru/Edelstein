using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Plugs;

public abstract class AbstractUserOnDisconnectPlug<TStageUser> : IPipelinePlug<UserOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountWorldRepository _accountWorldRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ISessionService _session;

    protected AbstractUserOnDisconnectPlug(
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

    public virtual async Task Handle(IPipelineContext ctx, UserOnDisconnect<TStageUser> message)
    {
        if (message.User.Stage != null)
            await message.User.Stage.Leave(message.User);

        if (message.User.AccountWorld != null)
            await _accountWorldRepository.Update(message.User.AccountWorld);

        if (message.User.Character != null)
        {
            if (!message.User.IsMigrating)
                message.User.Character.TemporaryStats.Records.Clear();
            await _characterRepository.Update(message.User.Character);
        }

        if (message.User.Account != null)
        {
            await _accountRepository.Update(message.User.Account);
            if (!message.User.IsMigrating)
                await _session.End(new SessionEndRequest(message.User.Account.ID));
        }
    }
}
