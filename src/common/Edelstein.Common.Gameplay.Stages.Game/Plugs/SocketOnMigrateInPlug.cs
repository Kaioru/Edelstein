using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Services.Migration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnMigrateInPlug : AbstractSocketOnMigrateInPlug<IGameStageUser, IGameStage, IGameContextOptions>
{
    public SocketOnMigrateInPlug(
        ILogger<AbstractSocketOnMigrateInPlug<IGameStageUser, IGameStage, IGameContextOptions>> logger,
        IMigrationService migrationService,
        IGameStage stage,
        IGameContextOptions options
    ) : base(logger, migrationService, stage, options)
    {
    }
}
