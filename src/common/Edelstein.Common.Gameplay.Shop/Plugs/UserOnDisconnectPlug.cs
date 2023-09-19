using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<IShopStageUser>
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
