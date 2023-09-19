using Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Plugs;

public abstract class AbstractUserOnDisconnectPlug<TStageUser> : IPipelinePlug<UserOnDisconnect<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly ISessionService _session;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountWorldRepository _accountWorldRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IFriendService _friendService;
    private readonly IPartyService _partyService;

    protected AbstractUserOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository, 
        IFriendService friendService, 
        IPartyService partyService
    )
    {
        _session = session;
        _accountRepository = accountRepository;
        _accountWorldRepository = accountWorldRepository;
        _characterRepository = characterRepository;
        _friendService = friendService;
        _partyService = partyService;
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
            {
                new ModifyTemporaryStatContext(message.User.Character.TemporaryStats).ResetAll();
                if (message.User.Party != null)
                    _ = _partyService.UpdateChannelOrField(new PartyUpdateChannelOrFieldRequest(
                        message.User.Party.ID,
                        message.User.Character.ID,
                        -2,
                        999999999
                    ));
            }

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
