using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Trade.Plugs;

public class UserMigratePlug : AbstractUserMigratePlug<ITradeStage, ITradeStageUser>
{
    public UserMigratePlug(
        ILogger<AbstractUserMigratePlug<ITradeStage, ITradeStageUser>> logger,
        ITradeStage stage,
        IMigrationService migrationService
    ) : base(logger, stage, migrationService)
    {
    }
}
