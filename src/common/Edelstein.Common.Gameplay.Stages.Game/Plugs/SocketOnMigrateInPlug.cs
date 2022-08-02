using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnMigrateInPlug : AbstractSocketOnMigrateInPlug<IGameStageUser, IGameStage, IGameContextOptions>
{
    public SocketOnMigrateInPlug(
        ILogger<AbstractSocketOnMigrateInPlug<IGameStageUser, IGameStage, IGameContextOptions>> logger,
        IMigrationService migrationService,
        ISessionService sessionService,
        IGameStage stage,
        IGameContextOptions options
    ) : base(logger, migrationService, sessionService, stage, options)
    {
    }
}
