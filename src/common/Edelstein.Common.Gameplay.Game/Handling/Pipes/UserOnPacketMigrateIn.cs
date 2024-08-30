using Edelstein.Common.Gameplay.Handling.Pipes;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketMigrateIn(
    IMigrationService migrations, 
    ISessionService sessions
) : BaseUserOnPacketMigrateIn<IGameStageSystem, IGameStageSystemUser>(migrations, sessions);
