using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Trade.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<ITradeStage, ITradeStageUser>
{
    public UserOnPacketMigrateInPlug(ILogger<AbstractUserOnPacketMigrateInPlug<ITradeStage, ITradeStageUser>> logger, ITradeStage stage, IMigrationService migrationService, ISessionService sessionService) : base(logger, stage, migrationService, sessionService)
    {
    }

    public override void SetValues(ITradeStageUser user, IMigration migration)
        => user.FromServerID = migration.FromServerID;
}
