using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnDisconnect(
    IAccountRepository accounts,
    IAccountWorldDataRepository accountWorldData,
    ICharacterRepository characters,
    ISessionService sessions
) : BaseUserOnDisconnect<IGameStageSystem, IGameStageSystemUser>(accounts, accountWorldData, characters, sessions);
