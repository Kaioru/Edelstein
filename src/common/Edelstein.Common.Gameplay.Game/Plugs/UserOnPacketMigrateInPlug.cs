using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<IGameStage, IGameStageUser>
{
    public UserOnPacketMigrateInPlug(ILogger<AbstractUserOnPacketMigrateInPlug<IGameStage, IGameStageUser>> logger, IGameStage stage, IMigrationService migrationService, ISessionService sessionService) : base(logger, stage, migrationService, sessionService)
    {
    }
}
