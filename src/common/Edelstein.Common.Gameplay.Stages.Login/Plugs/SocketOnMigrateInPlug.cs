using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SocketOnMigrateInPlug : AbstractSocketOnMigrateInPlug<ILoginStageUser, ILoginStage, ILoginContextOptions>
{
    public SocketOnMigrateInPlug(
        ILogger<AbstractSocketOnMigrateInPlug<ILoginStageUser, ILoginStage, ILoginContextOptions>> logger,
        IMigrationService migrationService,
        ISessionService sessionService,
        ILoginStage stage,
        ILoginContextOptions options
    ) : base(logger, migrationService, sessionService, stage, options)
    {
    }
}
