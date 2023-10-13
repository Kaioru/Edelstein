using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class UserMigratePlug : AbstractUserMigratePlug<IGameStage, IGameStageUser>
{
    public UserMigratePlug(
        ILogger<AbstractUserMigratePlug<IGameStage, IGameStageUser>> logger,
        IGameStage stage,
        IMigrationService migrationService
    ) : base(logger, stage, migrationService)
    {
    }
}
