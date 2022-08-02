using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SocketOnDisconnectPlug : AbstractSocketOnDisconnectPlug<ILoginStageUser>
{
    public SocketOnDisconnectPlug(
        ISessionService session,
        IAccountRepository accountRepository,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    ) : base(session, accountRepository, accountWorldRepository, characterRepository)
    {
    }
}
