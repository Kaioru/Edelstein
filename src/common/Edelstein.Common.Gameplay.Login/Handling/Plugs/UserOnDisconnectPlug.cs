using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<ILoginStageUser>
{
    public UserOnDisconnectPlug(
        ISessionService session, 
        IAccountRepository accountRepository, 
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository, 
        IFriendService friendService, 
        IPartyService partyService
    ) : base(session, accountRepository, accountWorldRepository, characterRepository, friendService, partyService)
    {
    }
}
