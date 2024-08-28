using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnDisconnect(
    IAccountRepository accounts,
    IAccountWorldDataRepository accountWorldData,
    ICharacterRepository characters,
    ISessionService sessions
) : BaseUserOnDisconnectPipe<ILoginStageSystem, ILoginStageSystemUser>(accounts, accountWorldData, characters, sessions);
