using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Services.Migration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnMigrateOutPlug : AbstractSocketOnMigrateOutPlug<IGameStageUser, IGameContextOptions>
{
    public SocketOnMigrateOutPlug(
        ILogger<AbstractSocketOnMigrateOutPlug<IGameStageUser, IGameContextOptions>> logger,
        IMigrationService migrationService,
        IGameContextOptions options
    ) : base(logger, migrationService, options)
    {
    }
}
