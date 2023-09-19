using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Gameplay.Trade.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<ITradeStageUser>
{
    public UserOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    ) : base(session, accountRepository, accountWorldRepository, characterRepository)
    {
    }
}
