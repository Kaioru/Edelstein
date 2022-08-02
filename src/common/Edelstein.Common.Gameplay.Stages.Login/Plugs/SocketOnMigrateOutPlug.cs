using Edelstein.Common.Gameplay.Stages.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Services.Migration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SocketOnMigrateOutPlug : AbstractSocketOnMigrateOutPlug<ILoginStageUser, ILoginContextOptions>
{
    public SocketOnMigrateOutPlug(
        ILogger<AbstractSocketOnMigrateOutPlug<ILoginStageUser, ILoginContextOptions>> logger,
        IMigrationService migrationService,
        ILoginContextOptions options
    ) : base(logger, migrationService, options)
    {
    }
}
