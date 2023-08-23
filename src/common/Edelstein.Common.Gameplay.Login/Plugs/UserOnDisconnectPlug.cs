using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<ILoginStageUser>
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
