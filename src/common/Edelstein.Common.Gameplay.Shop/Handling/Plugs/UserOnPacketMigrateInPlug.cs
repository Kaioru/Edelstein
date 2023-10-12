using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<IShopStage, IShopStageUser>
{
    public UserOnPacketMigrateInPlug(
        ILogger<AbstractUserOnPacketMigrateInPlug<IShopStage, IShopStageUser>> logger,
        IShopStage stage,
        IMigrationService migrationService,
        ISessionService sessionService, 
        IFriendService friendService, 
        IPartyService partyService
    ) : base(logger, stage, migrationService, sessionService, friendService, partyService)
    {
    }

    public override void SetValues(IShopStageUser user, IMigration migration)
        => user.FromServerID = migration.FromServerID;
}
