using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnDisconnectPlug : AbstractSocketOnDisconnectPlug<IGameStageUser>
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
