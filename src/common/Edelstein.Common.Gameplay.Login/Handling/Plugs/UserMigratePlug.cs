﻿using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserMigratePlug : AbstractUserMigratePlug<ILoginStage, ILoginStageUser>
{
    public UserMigratePlug(
        ILogger<AbstractUserMigratePlug<ILoginStage, ILoginStageUser>> logger,
        ILoginStage stage,
        IMigrationService migrationService
    ) : base(logger, stage, migrationService)
    {
    }
}
