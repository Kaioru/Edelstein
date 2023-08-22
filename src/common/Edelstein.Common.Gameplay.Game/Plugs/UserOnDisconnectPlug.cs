using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class UserOnDisconnectPlug : AbstractUserOnDisconnectPlug<IGameStageUser>
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
