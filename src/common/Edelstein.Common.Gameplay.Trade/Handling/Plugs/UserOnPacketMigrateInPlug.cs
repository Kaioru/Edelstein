using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Trade.Handling.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<ITradeStage, ITradeStageUser>
{
    public UserOnPacketMigrateInPlug(
        ILogger<AbstractUserOnPacketMigrateInPlug<ITradeStage, ITradeStageUser>> logger,
        ITradeStage stage,
        IMigrationService migrationService,
        ISessionService sessionService, 
        IFriendService friendService, 
        IPartyService partyService
    ) : base(logger, stage, migrationService, sessionService, friendService, partyService)
    {
    }

    public override void SetValues(ITradeStageUser user, IMigration migration)
        => user.FromServerID = migration.FromServerID;
}
